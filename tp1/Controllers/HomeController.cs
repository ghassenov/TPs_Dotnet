using Microsoft.AspNetCore.Mvc;

namespace tp1.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

