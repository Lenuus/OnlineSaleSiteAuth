using Microsoft.AspNetCore.Mvc;

namespace OnlineSaleSiteAuth.Controllers
{
    public class AnimationController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult WaveAnimation()
        {
            return View();
        }
    }
}
