using BasicProject.Entity;
using BasicProject.Models;
using BasicProject.Services;
using Microsoft.AspNetCore.Mvc;
using RotativaCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BasicProject.Controllers
{
    public class PayController : Controller
    {
        private readonly IpayComputeService _payComputeService;
        private readonly IEmployeeService _employeeService;
        private readonly ITaxService _taxService;
        private readonly INationalInsuranceContributionService _nationalInsuranceContributionService;
        private decimal overTimeHours;
        private decimal contractualEarning;
        private decimal overtimeEarning;
        private decimal totalEarn;
        private decimal tax;
        private decimal unionFee;
        private decimal studentLoan;
        private decimal nationalInsurance;
        private decimal totalDeduction;

        public PayController(IpayComputeService payComputeService, IEmployeeService employeeService, ITaxService taxService, INationalInsuranceContributionService nationalInsuranceContributionService)
        {
            _payComputeService = payComputeService;
            _employeeService = employeeService;
            _taxService = taxService;
            _nationalInsuranceContributionService = nationalInsuranceContributionService; 

        }
        public IActionResult Index()
        {
            var payRecords = _payComputeService.GetAll().Select(pay => new PaymentRecordIndexViewModel
            {
                Id= pay.Id,
                EmployeeId = pay.EmployeeId,
                FullName = pay.FullName,
                PayDate = pay.PayDate,
                PayMonth = pay.PayMonth,
                TaxYearId = pay.TaxYearId,
                Year = _payComputeService.GetTaxYearById(pay.TaxYearId).YearofTax,
                TotalEarnings = pay.TotalEarnings,
                TotalDeducation = pay.TotalDeduction,
                NetPayment = pay.NetPayment,
                Employee = pay.Employee
            });
            return View(payRecords);
        }
        public IActionResult Create()
        {
            ViewBag.employees = _employeeService.GetAllEmployeesForPayroll();
            ViewBag.taxYears = _payComputeService.GetAllTaxYear();
            var model = new PaymentRecordCreateViewModel();
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PaymentRecordCreateViewModel model)
        {
            if(ModelState.IsValid)
            {
                var payrecord = new PaymentRecord()
                {
                    Id = model.Id,
                    EmployeeId = model.EmployeeId,
                    FullName = _employeeService.GetById(model.EmployeeId).FullName,
                    NiNo = _employeeService.GetById(model.EmployeeId).NationalInsuranceNo,
                    PayDate = model.PayDate,
                    PayMonth = model.PayMonth,
                    TaxYearId = model.TaxYearId,
                    TaxCode = model.TaxCode,
                    HourlyRate = model.HourlyRate,
                    HoursWorked = model.HoursWorked,
                    ContractualHours = model.ContractualHours,
                    OverTimeHours = overTimeHours = _payComputeService.OvertimeHours(model.HoursWorked, model.ContractualHours),
                    ContractualEarnings = contractualEarning = _payComputeService.ContractualEarnings(model.ContractualHours, model.HoursWorked, model.HourlyRate),
                    OvertimeEarnings = overtimeEarning = _payComputeService.OvertimeEarnings(_payComputeService.OvertimeRate(model.HourlyRate), overTimeHours),
                    TotalEarnings = totalEarn = _payComputeService.TotalEarnings(overtimeEarning, contractualEarning),
                    Tax = tax = _taxService.TaxAmount(totalEarn),
                    UnionFee = unionFee = _employeeService.UnionFees(model.EmployeeId),
                    SLC = studentLoan = _employeeService.StudentLoanRepaymentAmount(model.EmployeeId, totalEarn),
                    NIC = nationalInsurance = _nationalInsuranceContributionService.NIContribution(totalEarn),
                    TotalDeduction = totalDeduction = _payComputeService.TotalDeduction(tax, nationalInsurance, studentLoan, unionFee),
                    NetPayment = _payComputeService.NetPay(totalEarn, totalDeduction)
                };
                await _payComputeService.CreateAsync(payrecord);
                return RedirectToAction(nameof(Index));
            }
            ViewBag.employees = _employeeService.GetAllEmployeesForPayroll();
            ViewBag.taxYears = _payComputeService.GetAllTaxYear();
            return View();
        }
        public IActionResult Detail(int id)
        {
            var paymentRecord = _payComputeService.GetById(id);
            if(paymentRecord == null)
            {
                return NotFound();
            }
            var model = new PaymentRecordDetailViewModel()
            {
                Id = paymentRecord.Id,
                EmployeeId = paymentRecord.EmployeeId,
                FullName = paymentRecord.FullName,
                NiNo = paymentRecord.NiNo,
                PayDate = paymentRecord.PayDate,
                PayMonth = paymentRecord.PayMonth,
                TaxYearId = paymentRecord.TaxYearId,
                Year = _payComputeService.GetTaxYearById(paymentRecord.TaxYearId).YearofTax,
                TaxCode = paymentRecord.TaxCode,
                HourlyRate = paymentRecord.HourlyRate,
                HoursWorked = paymentRecord.HoursWorked,
                ContractualHours = paymentRecord.ContractualHours,
                OverTimeHours = paymentRecord.OverTimeHours,
                OverTimeRate = _payComputeService.OvertimeRate(paymentRecord.HourlyRate),
                OvertimeEarnings = paymentRecord.OvertimeEarnings,
                Tax = paymentRecord.Tax,
                NIC = paymentRecord.NIC,
                UnionFee = paymentRecord.UnionFee,
                SLC = paymentRecord.SLC,
                TotalEarnings = paymentRecord.TotalEarnings,
                TotalDeduction = paymentRecord.TotalDeduction,
                Employee = paymentRecord.Employee,
                TaxYear = paymentRecord.TaxYear,
                NetPayment = paymentRecord.NetPayment
            };
            return View(model);
        }

        [HttpGet]
        public IActionResult PaySlip(int id)
        {
            var paymentRecord = _payComputeService.GetById(id);
            if (paymentRecord == null)
            {
                return NotFound();
            }
            var model = new PaymentRecordDetailViewModel()
            {
                Id = paymentRecord.Id,
                EmployeeId = paymentRecord.EmployeeId,
                FullName = paymentRecord.FullName,
                NiNo = paymentRecord.NiNo,
                PayDate = paymentRecord.PayDate,
                PayMonth = paymentRecord.PayMonth,
                TaxYearId = paymentRecord.TaxYearId,
                Year = _payComputeService.GetTaxYearById(paymentRecord.TaxYearId).YearofTax,
                TaxCode = paymentRecord.TaxCode,
                HourlyRate = paymentRecord.HourlyRate,
                HoursWorked = paymentRecord.HoursWorked,
                ContractualHours = paymentRecord.ContractualHours,
                OverTimeHours = paymentRecord.OverTimeHours,
                OverTimeRate = _payComputeService.OvertimeRate(paymentRecord.HourlyRate),
                OvertimeEarnings = paymentRecord.OvertimeEarnings,
                Tax = paymentRecord.Tax,
                NIC = paymentRecord.NIC,
                UnionFee = paymentRecord.UnionFee,
                SLC = paymentRecord.SLC,
                TotalEarnings = paymentRecord.TotalEarnings,
                TotalDeduction = paymentRecord.TotalDeduction,
                Employee = paymentRecord.Employee,
                TaxYear = paymentRecord.TaxYear,
                NetPayment = paymentRecord.NetPayment
            };
            return View(model);
        }

        public IActionResult GeneratePaySlipPdf(int id)
        {
            var payslip = new ActionAsPdf("PaySlip", new { id = id })
            {
                FileName = "payslip.pdf"
            };
            return payslip;
        }
    }
}
