using Microsoft.AspNetCore.Mvc;

namespace NeoBankWebApp.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
