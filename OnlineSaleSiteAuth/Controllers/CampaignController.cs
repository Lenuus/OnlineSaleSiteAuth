using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OnlineSaleSiteAuth.Application.Service.Campaign;
using OnlineSaleSiteAuth.Application.Service.Campaign.Dtos;
using OnlineSaleSiteAuth.Application.Service.Product;
using OnlineSaleSiteAuth.Models.Campaign;

namespace OnlineSaleSiteAuth.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CampaignController : Controller
    {
        private readonly IMapper _mapper;
        private readonly ICampaignService _campaignService;
        private readonly IProductService _productService;

        public CampaignController(ICampaignService campaignService, IMapper mapper, IProductService productService)
        {
            _campaignService = campaignService;
            _mapper = mapper;
            _productService = productService;
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var products = await _productService.GetAllProductIds().ConfigureAwait(false);
            var productList = products.Data.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Name
            }).ToList();
            ViewBag.Products = new SelectList(productList, "Value", "Text");
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(AddCampaignModel request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var requestMapped = _mapper.Map<AddCampaignDto>(request);
            await _campaignService.AddCampaign(requestMapped).ConfigureAwait(false);
            return RedirectToAction("Index","Product");   
        }
    }
}
