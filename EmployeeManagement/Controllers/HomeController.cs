using EmployeeManagement.Models;
using EmployeeManagement.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement.Controllers
{

    public class HomeController : Controller
    {
        private readonly IEmployeeRepository employeeRepository;
        private readonly IHostingEnvironment hostingEnvironment;

        public HomeController(IEmployeeRepository employeeRepository, IHostingEnvironment hostingEnvironment)
        {
            this.employeeRepository = employeeRepository;
            this.hostingEnvironment = hostingEnvironment;

        }


        public ViewResult Index()
        {
            var model = employeeRepository.GetEmployees();
            return View(model);
        }


        public ViewResult Details(int? id)
        {
            HomeDetailsViewModel homeDetailsViewModel = new HomeDetailsViewModel()
            { Employee = employeeRepository.GetEmployee(id ?? 1), PageTitle = "Employee Details" };
            return View(homeDetailsViewModel);
        }

        [HttpGet]
        public ViewResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(EmployeeCreateViewModel model)
        {            
            if(ModelState.IsValid)
            {
                string uniqueFileName = ProcessUploadedFile(model);
               
                Employee employee = new Employee { Name = model.Name, Email = model.Email, Department = model.Department, PhotoPath = uniqueFileName };

                employeeRepository.Add(employee);
                return RedirectToAction("details", new { id = employee.Id });
            }

            return View();
        }

        [HttpGet]
        public ViewResult Edit(int id)
        {
            Employee employee = employeeRepository.GetEmployee(id);

            EmployeeEditViewModel employeeEditViewModel = new EmployeeEditViewModel()
            {
                Id = employee.Id,
                Name = employee.Name,
                Email = employee.Email,
                Department = employee.Department,
                ExistingPhotoPath = employee.PhotoPath
            };

            return View(employeeEditViewModel);
        }


        [HttpPost]
        public IActionResult Edit(EmployeeEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                Employee employee = employeeRepository.GetEmployee(model.Id);
                employee.Name = model.Name;
                employee.Email = model.Email;
                employee.Department = model.Department;

                if (model.Photo != null)
                {
                    if (model.ExistingPhotoPath != null)
                    {
                       string filePath= Path.Combine(hostingEnvironment.WebRootPath, "images", model.ExistingPhotoPath);

                        System.IO.File.Delete(filePath);
                    }
                    employee.PhotoPath = ProcessUploadedFile(model);
                }

                employeeRepository.Update(employee);
                return RedirectToAction("details", new { id = employee.Id });
            }

            return View();
        }

        
        public ActionResult Delete(int id)
        {
            Employee employee = employeeRepository.GetEmployee(id);

            if (employee.PhotoPath != null)
            {
                string filePath = Path.Combine(hostingEnvironment.WebRootPath, "images", employee.PhotoPath);

                System.IO.File.Delete(filePath);               
            }
            employeeRepository.Detele(id);

            return RedirectToAction("index");
        }


        private string ProcessUploadedFile(EmployeeCreateViewModel model)
        {
            string uniqueFileName = null;

            if (model.Photo != null )
            {               
                    string uploadFolder = Path.Combine(hostingEnvironment.WebRootPath, "images");
                    uniqueFileName = Guid.NewGuid().ToString() + "_" + model.Photo.FileName.Split('\\').Last();

                    string filePath = Path.Combine(uploadFolder, uniqueFileName);
                using(var fileStream=new FileStream(filePath, FileMode.Create))
                {
                    model.Photo.CopyTo(fileStream);
                }                              
            }

            return uniqueFileName;
        }


        //public ViewResult ViewBag1()
        //{
        //    Employee model = employeeRepository.GetEmployee(2);
        //    ViewBag.Employee = model;
        //    ViewBag.PageTitle = "Employee Details";
        //    return View(model);
        //}

        //public ViewResult StronglyTyped()
        //{
        //    Employee model = employeeRepository.GetEmployee(2);
        //    ViewBag.PageTitle = "Strongly Typed Employee Details";
        //    return View(model);
        //}
    }
}
