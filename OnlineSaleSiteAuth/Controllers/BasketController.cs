using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OnlineSaleSiteAuth.Application.Service.Basket;
using OnlineSaleSiteAuth.Application.Service.Basket.Dtos;
using OnlineSaleSiteAuth.Domain;
using OnlineSaleSiteAuth.Models.Basket;

namespace OnlineSaleSiteAuth.Controllers
{
    public class BasketController : Controller
    {
        private readonly IBasketService _basketService;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        public BasketController(IBasketService basketService, IMapper mapper, UserManager<User> userManager)
        {
            _basketService = basketService;
            _mapper = mapper;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var response = _basketService.GetAllBasket();
            if (response == null)
            {
                return NotFound();
            }
            var mappedResponse = _mapper.Map<List<BasketListModel>>(response.Result.Data);
            return View(mappedResponse);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] BasketListSessionModel request)
        {
            var mappedProduct = _mapper.Map<BasketListSessionDto>(request);
            var response = await _basketService.AddToBasket(mappedProduct.ProductId, mappedProduct.Quantity).ConfigureAwait(false);
            if (!response.IsSuccessful)
            {
                return BadRequest(response);
            }
            return Ok();
        }
        [HttpPost]
        public async Task<IActionResult> Delete([FromBody] BasketListSessionModel product)
        {
            var mappedProduct = _mapper.Map<BasketListSessionDto>(product);
            var response = await _basketService.RemoveFromBasket(mappedProduct.ProductId, mappedProduct.Quantity).ConfigureAwait(false);
            if (!response.IsSuccessful)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        public async Task<IActionResult> GetCount()
        {
            int totalCount = 0;
            var productCount = await _basketService.GetAllBasket().ConfigureAwait(false);
            if (productCount.Data != null)
            {
                totalCount = productCount.Data.Sum(item => item.Quantity);

            }
            ViewBag.TotalItemCount = totalCount;
            return Ok(totalCount);
        }
        [HttpDelete]
        public async Task<IActionResult> Clear()
        {
            var Clearing = await _basketService.ClearBasket().ConfigureAwait(false);
            if (!Clearing.IsSuccessful)
            {
                return NotFound();
            }
            return Ok(Clearing);
        }
    }
}
