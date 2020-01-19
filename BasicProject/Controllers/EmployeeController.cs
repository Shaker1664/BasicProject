using BasicProject.Entity;
using BasicProject.Models;
using BasicProject.Services;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BasicProject.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IEmployeeService _employeeServivce;
        private readonly HostingEnvironment _hostingEnvironment;
        public EmployeeController(IEmployeeService employeeService, HostingEnvironment hostingEnvironment)
        {
            _employeeServivce = employeeService;
            _hostingEnvironment = hostingEnvironment;
        }

        public IActionResult Index()
        {
            var employees = _employeeServivce.GetAll().Select(employee => new EmloyeeIndexViewModel
            {
                Id = employee.Id,
                EmployeeNo=employee.EmployeeNo,
                FullName=employee.FullName,
                ImageUrl=employee.ImageUrl,
                Gender=employee.Gender,
                Designation=employee.Designation,
                City=employee.City,
                DateJoined=employee.DateJoined
            }).ToList();
            return View(employees);
        }
        [HttpGet]
        public IActionResult Create()
        {
            var model = new EmployeeCreateViewModel();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken] //prevents cross-site request forgery attacks
        public async Task<IActionResult> Create(EmployeeCreateViewModel model)
        {
            if(ModelState.IsValid)
            {
                var employee = new Employee
                {
                    Id = model.Id,
                    EmployeeNo= model.EmployeeNo,
                    FirstName=model.FirstName,
                    LastName=model.LastName,
                    FullName=model.FullName,
                    Gender=model.Gender,
                    Email=model.Email,
                    DOB=model.DOB,
                    DateJoined=model.DateJoined,
                    NationalInsuranceNo=model.NationalInsuranceNo,
                    Paymentmethod=model.Paymentmethod,
                    StudentLoan=model.StudentLoan,
                    PhoneNumber=model.PhoneNumber,
                    Unionmember=model.Unionmember,
                    Address=model.Address,
                    City=model.City,
                    Postcode=model.Postcode,
                    Designation=model.Designation
                };
                if(model.ImageUrl !=null && model.ImageUrl.Length > 0)
                {
                    var uploadDir = @"images/employee";
                    var fileName = Path.GetFileNameWithoutExtension(model.ImageUrl.FileName);
                    var extension = Path.GetExtension(model.ImageUrl.FileName);
                    var webRootPath = _hostingEnvironment.WebRootPath;
                    fileName = DateTime.UtcNow.ToString("yymmssfff") + fileName + extension;
                    var path = Path.Combine(webRootPath, uploadDir, fileName);
                    await model.ImageUrl.CopyToAsync(new FileStream(path, FileMode.Create));
                    employee.ImageUrl = "/" + uploadDir + "/" + fileName;    
                }
                await _employeeServivce.CreateAsync(employee);
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        public IActionResult Edit(int id)
        {
            var employee = _employeeServivce.GetById(id);
            if(employee == null)
            {
                return NotFound();
            }

            var model = new EmployeeEditViewModel()
            {
                Id = employee.Id,
                EmployeeNo = employee.EmployeeNo,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                Gender = employee.Gender,
                Email = employee.Email,
                DOB = employee.DOB,
                DateJoined = employee.DateJoined,
                NationalInsuranceNo = employee.NationalInsuranceNo,
                Paymentmethod = employee.Paymentmethod,
                StudentLoan = employee.StudentLoan,
                PhoneNumber = employee.PhoneNumber,
                Unionmember = employee.Unionmember,
                Address = employee.Address,
                City = employee.City,
                Postcode = employee.Postcode,
                Designation = employee.Designation
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EmployeeEditViewModel model)
        {
            if(ModelState.IsValid)
            {
                var employee = _employeeServivce.GetById(model.Id);
                if (employee == null)
                {
                    return NotFound();
                }
                employee.EmployeeNo = model.EmployeeNo;
                employee.FirstName = model.FirstName;
                employee.LastName = model.LastName;
                employee.MiddleName = model.MiddleName;
                employee.NationalInsuranceNo = model.NationalInsuranceNo;
                employee.Gender = model.Gender;
                employee.Email = model.Email;
                employee.DOB = model.DOB;
                employee.DateJoined = model.DateJoined;
                employee.PhoneNumber = model.PhoneNumber;
                employee.Designation = model.Designation;
                employee.Paymentmethod = model.Paymentmethod;
                employee.StudentLoan = model.StudentLoan;
                employee.Unionmember = model.Unionmember;
                employee.Address = model.Address;
                employee.City = model.City;
                employee.Postcode = model.Postcode;
                
                if(model.ImageUrl != null && model.ImageUrl.Length > 0)
                {
                    var uploadDir = @"images/employee";
                    var fileName = Path.GetFileNameWithoutExtension(model.ImageUrl.FileName);
                    var extension = Path.GetExtension(model.ImageUrl.FileName);
                    var webRootPath = _hostingEnvironment.WebRootPath;
                    fileName = DateTime.UtcNow.ToString("yymmssfff") + fileName + extension;
                    var path = Path.Combine(webRootPath, uploadDir, fileName);
                    await model.ImageUrl.CopyToAsync(new FileStream(path, FileMode.Create));
                    employee.ImageUrl = "/" + uploadDir + "/" + fileName;
                }

                await _employeeServivce.UpdateAsync(employee);
                return RedirectToAction(nameof(Index));
            }

            return View();
        }

        [HttpGet]
        public IActionResult Detail(int id)
        {
            var employee = _employeeServivce.GetById(id);
            if(employee==null)
            {
                return NotFound();
            }
            EmployeeDetailViewModel model = new EmployeeDetailViewModel()
            {
                Id = employee.Id,
                EmployeeNo = employee.EmployeeNo,
                FullName = employee.FullName,
                Gender = employee.Gender,
                DOB = employee.DOB,
                DateJoined = employee.DateJoined,
                Designation = employee.Designation,
                NationalInsuranceNo = employee.NationalInsuranceNo,
                PhoneNumber = employee.PhoneNumber,
                Email = employee.Email,
                Paymentmethod = employee.Paymentmethod,
                StudentLoan = employee.StudentLoan,
                Unionmember = employee.Unionmember,
                Address = employee.Address,
                City = employee.City,
                ImageUrl = employee.ImageUrl,
                Postcode = employee.Postcode
            };
            return View(model);
        } 

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var employee = _employeeServivce.GetById(id);
            if(employee == null)
            {
                return NotFound();
            }

            var model = new EmployeeDeleteViewModel()
            {
                Id = employee.Id,
                FullName = employee.FullName
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(EmployeeDeleteViewModel model)
        {
            await _employeeServivce.Delete(model.Id);
            return RedirectToAction(nameof(Index));
        }
    }
}