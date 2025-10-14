using Microsoft.AspNetCore.Mvc;

namespace CamCook.Controllers
{
    public class SplashController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        //public IActionResult Index() => View();
    }
}
