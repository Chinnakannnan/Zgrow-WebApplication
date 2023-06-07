using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NeoBankWebApp.Rule_Common;

namespace NeoBankWebApp.Controllers
{
 
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            string user = HttpContext.Session.GetString(Variables.UserType);
            if (user != "2") { return RedirectToAction("Login", "Login"); }


            return View();
        }
    }
}
