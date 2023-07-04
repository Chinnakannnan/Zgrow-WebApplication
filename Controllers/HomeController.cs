using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NeoBankWebApp.Models;
using NeoBankWebApp.Rule_Common;
using System.Diagnostics;

namespace NeoBankWebApp.Controllers
{
 
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        { 
            if (HttpContext.Session.GetString(Variables.UserType) != "3")
                return RedirectToAction("Login", "Login");
            if (HttpContext.Session.GetString(Variables.CustomerID) == null)
                return RedirectToAction("Login", "Login");

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}