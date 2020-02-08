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
            //if (ModelState.IsValid)
            //    {
            //        Employee newEmployee = employeeRepository.Add(employee);
            //        return RedirectToAction("details", new { id = newEmployee.Id });
            //    }
            //    else
            //    {
            //        return View();
            //    }
            //if (ModelState.IsValid)
            //{

            //    Employee newEmployee = employeeRepository.Add(employee);                
            //}

            if(ModelState.IsValid)
            {
                string uniqueFileName = null;

                if (model.Photos != null && model.Photos.Count > 0)
                {
                    foreach (IFormFile photo in model.Photos)
                    {


                        string uploadFolder = Path.Combine(hostingEnvironment.WebRootPath, "images");
                        uniqueFileName = Guid.NewGuid().ToString() + "_" + photo.FileName.Split('\\').Last();

                        string filePath = Path.Combine(uploadFolder, uniqueFileName);

                       photo.CopyTo(new FileStream(filePath, FileMode.Create));
                    }
                }
                Employee employee = new Employee { Name = model.Name, Email = model.Email, Department = model.Department, PhotoPath = uniqueFileName };

                employeeRepository.Add(employee);
                return RedirectToAction("details", new { id = employee.Id });
            }

            return View();
        }

        public ViewResult ViewBag1()
        {
            Employee model = employeeRepository.GetEmployee(2);
            ViewBag.Employee = model;
            ViewBag.PageTitle = "Employee Details";
            return View(model);
        }

        public ViewResult StronglyTyped()
        {
            Employee model = employeeRepository.GetEmployee(2);
            ViewBag.PageTitle = "Strongly Typed Employee Details";
            return View(model);
        }
    }
}
