using Microsoft.AspNetCore.Mvc;
using NeoBankWebApp.Rule_Common;

namespace NeoBankWebApp.Controllers
{
    public class PayoutController : Controller
    {
        public IActionResult Index()
        {
            if (HttpContext.Session.GetString(Variables.CustomerID) == null)
            {
                return RedirectToAction("Login", "Login");
            }
            return View();
        }
    }
}
