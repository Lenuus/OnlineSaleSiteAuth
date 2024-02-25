using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using OnlineSaleSiteAuth.Application.Service.CustomPage;
using OnlineSaleSiteAuth.Application.Service.CustomPage.Dtos;
using OnlineSaleSiteAuth.Models.CustomPage;

namespace OnlineSaleSiteAuth.Controllers
{
    public class CustomPageController : Controller
    {
        private readonly ICustomPageService _customPageService;
        private readonly IMapper _mapper;

        public CustomPageController(ICustomPageService customPageService, IMapper mapper)
        {
            _customPageService = customPageService;
            _mapper = mapper;
        }

        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateCustomPageModel model)
        {
            if (ModelState.IsValid)
            {
                var htmlDto = _mapper.Map<CreateCustomPageDto>(model);
                await _customPageService.CreateCustomPage(htmlDto);
                return View("Index", model);
            }
            return View();
        }
        public async Task<IActionResult> Index(Guid id)
        {
            var customPage = await _customPageService.GetCustomPage(id).ConfigureAwait(false);
            var mappedCustomPage = _mapper.Map<CreateCustomPageModel>(customPage.Data);
            return View(mappedCustomPage);

        }


    }
}
