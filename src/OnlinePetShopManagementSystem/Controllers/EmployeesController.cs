using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace OnlinePetShopManagementSystem.Controllers
{
    public class EmployeesController : Controller
    {
        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }
				public IActionResult Create()
				{
						return View();
				}
				public IActionResult Update()
				{
						return View();
				}
				public IActionResult ManagePhoto()
				{
						return View();
				}
				public IActionResult ManageAllEmployeeImagesInCloudinary() {
						return View();
				}


		}
}
