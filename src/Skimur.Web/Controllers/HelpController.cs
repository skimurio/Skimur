using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Skimur.Web.Controllers
{
    public class HelpController : Controller
    {

        public IActionResult Privacy()
        {
            ViewBag.ManageNavigationKey = "privacy";
            return View();
        }

        public IActionResult Terms()
        {
            return View();
        }
    }
}
