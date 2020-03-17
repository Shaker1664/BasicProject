using BasicProject.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BasicProject.Models
{
    public class PaymentRecordIndexViewModel
    {
        public int Id { get; set; }
        [ForeignKey("Employee")]
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }
        [Display(Name= "Name")]
        public string FullName { get; set; }
        [Display(Name = "Pay Date")]
        public DateTime PayDate { get; set; }
        [Display(Name = "Month")]
        public string PayMonth { get; set; }
        public int TaxYearId { get; set; }
        public string Year { get; set; }
        [Display(Name = "Total Earnings")]
        public decimal TotalEarnings { get; set; }
        [Display(Name = "Total Deduction")]
        public decimal TotalDeducation { get; set; }
        [Display(Name = "Net")]
        public decimal NetPayment { get; set; }

    }
}
