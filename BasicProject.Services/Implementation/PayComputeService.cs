using BasicProject.Entity;
using BasicProject.Persistence;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicProject.Services.Implementation
{
    public class PayComputeService : IpayComputeService
    {
        private readonly ApplicationDbContext _context;
        private decimal contractualEarnings;
        private decimal overtimeHours;

        public PayComputeService(ApplicationDbContext context)
        {
            _context = context;
        }
        public decimal ContractualEarnings(decimal contractualHours, decimal hoursWorked, decimal hourlyRate)
        {
            if(hoursWorked < contractualHours)
            {
                contractualEarnings = hoursWorked * hourlyRate;
            }
            else
            {
                contractualEarnings = contractualHours * hourlyRate;
            }
            return contractualEarnings;
        }

        public async Task CreateAsync(PaymentRecord paymentrecord)
        {
            await _context.PaymentRecords.AddAsync(paymentrecord);
            await _context.SaveChangesAsync();
        }

        public IEnumerable<PaymentRecord> GetAll() => _context.PaymentRecords.OrderBy(p => p.EmployeeId);

        public IEnumerable<SelectListItem> GetAllTaxYear()
        {
            var allTaxYear = _context.TaxYears.Select(taxYears => new SelectListItem
            {
                Text = taxYears.YearofTax,
                Value = taxYears.Id.ToString()
            });
            return allTaxYear;
        }

        public PaymentRecord GetById(int id) =>
        
            _context.PaymentRecords.Where(pay => pay.Id == id)
                .FirstOrDefault();

        public TaxYear GetTaxYearById(int id) => _context.TaxYears.Where(year => year.Id == id).FirstOrDefault();

        public decimal NetPay(decimal totalEarnings, decimal totalDeuction) => totalEarnings - totalDeuction;

        public decimal OvertimeEarnings(decimal overtimeRate, decimal overtimeHours) => overtimeHours * overtimeRate;

        public decimal OvertimeHours(decimal hoursWorked, decimal contractualHours)
        {
            if(hoursWorked <= contractualHours)
            {
                overtimeHours = 0.00m;
            }
            else
            {
                overtimeHours = hoursWorked - contractualHours;
            }
            return overtimeHours;
        }

        public decimal OvertimeRate(decimal hourlyRate) => hourlyRate * 1.5m;

        public decimal TotalDeduction(decimal Tax, decimal nic, decimal studentLoanRepayment, decimal unionfee)
            => Tax + nic + studentLoanRepayment + unionfee;

        public decimal TotalEarnings(decimal overtimeEarnings, decimal contractualEarnings)
            => overtimeEarnings + contractualEarnings;
    }
}
