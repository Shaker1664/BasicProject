using BasicProject.Entity;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BasicProject.Services
{
    public interface IpayComputeService
    {
        Task CreateAsync(PaymentRecord paymentrecord);
        PaymentRecord GetById(int id);
        TaxYear GetTaxYearById(int id);
        IEnumerable<PaymentRecord> GetAll();
        IEnumerable<SelectListItem> GetAllTaxYear();
        decimal OvertimeHours(decimal hoursWorked, decimal contractualHours);
        decimal ContractualEarnings(decimal contractualHours, decimal hoursWorked, decimal hourlyRate);
        decimal OvertimeRate(decimal hourlyRate);
        decimal OvertimeEarnings(decimal overtimeRate, decimal overtimeHours);
        decimal TotalEarnings(decimal overtimeEarnings, decimal contractualEarnings);
        decimal TotalDeduction(decimal Tax, decimal nic, decimal studentLoanRepayment, decimal unionfee);
        decimal NetPay(decimal totalEarnings, decimal totalDeuction);
    }
}
 