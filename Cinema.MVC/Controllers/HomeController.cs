using Microsoft.AspNetCore.Mvc;

namespace Cinema.MVC.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
