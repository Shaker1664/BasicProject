using BasicProject.Entity;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BasicProject.Models
{
    public class EmployeeCreateViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="Employee Number Required*"),
            RegularExpression("[A-Z]{3,3}[0-9]{3}$")]
        public string EmployeeNo { get; set; }
        [Required(ErrorMessage ="First Name Required"),StringLength(50, MinimumLength =4)]
        [RegularExpression(@"^[A-Z][a-zA-Z""'\s-]*$"), Display(Name ="First Name")]
        public string FirstName { get; set; }
        [StringLength(50), Display(Name ="Middle Name")]
        public string MiddleName { get; set; }
        [Required(ErrorMessage = "First Name Required"), StringLength(50, MinimumLength = 4)]
        [RegularExpression(@"^[A-Z][a-zA-Z""'\s-]*$"), Display(Name = "Last Name")]
        public string LastName { get; set; }
        public string FullName {
            get {
                return FirstName + (string.IsNullOrEmpty(MiddleName) ? " " : (" " + (char?)MiddleName[0] + ". ").ToUpper()) + LastName; 
        }   }
        public string Gender { get; set; }
        [Display(Name ="Photo")]
        public IFormFile ImageUrl { get; set; }
        [DataType(DataType.Date), Display(Name ="Date Of Birth")]
        public DateTime DOB { get; set; }
        [DataType(DataType.Date), Display(Name = "Joining Date")]
        public DateTime DateJoined { get; set; } = DateTime.UtcNow;
        [Required(ErrorMessage ="Job role Required"), StringLength(100)]
        public string Designation { get; set; }
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        [Required, StringLength(50), Display(Name ="NI No")]
        [RegularExpression(@"^[A-CEGHJ-PR-TW-Z]{1}[A-CEGHJ-NPR-TW-Z]{1}[0-9]{6}[A-D\s]$")]
        public string NationalInsuranceNo { get; set; }
        [Display(Name ="Payment Mewthod")]
        public paymentmethod Paymentmethod { get; set; }
        [Display(Name = "Student Loan")]
        public studentLoan StudentLoan { get; set; }
        [Display(Name = "Union Member")]
        public unionmember Unionmember { get; set; }
        [Required, StringLength(150)]
        public string Address { get; set; }
        [Required, StringLength(50)]
        public string City { get; set; }
        [Required, StringLength(50)]
        public string Postcode { get; set; }
    }
}
