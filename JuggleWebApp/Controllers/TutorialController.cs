using Microsoft.AspNetCore.Mvc;

namespace JuggleWebApp.Controllers
{
    public class TutorialController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Video()
        {
            return View();
        }

        public IActionResult Text()
        {
            return View();
        }
    }
}
