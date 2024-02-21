using Microsoft.AspNetCore.Mvc;
using OnlineSaleSiteAuth.Models.TinyMce;

namespace OnlineSaleSiteAuth.Controllers
{
    public class TinyMCEController : Controller
    {
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(TinyMCE model)
        {
            if (ModelState.IsValid)
            {
                var htmlModel = new TinyMCE { HtmlContent = model.HtmlContent };
                return View("Index", htmlModel);
            }
            return View();
        }

    }
}
