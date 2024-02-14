using AutoMapper;
using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.IdentityModel.Tokens;
using OnlineSaleSiteAuth.Application.Service.Basket;
using OnlineSaleSiteAuth.Application.Service.Category;
using OnlineSaleSiteAuth.Application.Service.Coupon;
using OnlineSaleSiteAuth.Application.Service.Coupon.Dtos;
using OnlineSaleSiteAuth.Application.Service.Product;
using OnlineSaleSiteAuth.Application.Service.Product.Dtos;
using OnlineSaleSiteAuth.Common.Helpers;
using OnlineSaleSiteAuth.Domain;
using OnlineSaleSiteAuth.Models.Coupon;

namespace OnlineSaleSiteAuth.Controllers
{
    public class CouponController : Controller
    {
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly ICouponService _couponService;
        private readonly IProductService _productService;

        public CouponController(IProductService productService, ICouponService couponService, UserManager<User> userManager, IMapper mapper)
        {
            _productService = productService;
            _couponService = couponService;
            _userManager = userManager;
            _mapper = mapper;
        }
        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Create(AddCouponRequestModel request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var mappedRequest = _mapper.Map<AddCouponRequestDto>(request);
            await _couponService.AddCoupon(mappedRequest).ConfigureAwait(false);
            return RedirectToAction("Index", "Product");
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> CouponGenerator()
        {
            var coupon = CouponCreaterHelper.GenerateCouponCode();
            var check = await _couponService.CheckCoupon(coupon).ConfigureAwait(false);
            if (!check.IsSuccessful)
            {
                return BadRequest(check.ErrorMessage);
            }
            return Ok(coupon);
        }

        public async Task<IActionResult> ApplyCoupon([FromBody] string Coupon)
        {
            var response = await _couponService.ApplyCoupon(Coupon).ConfigureAwait(false);

            if (response.IsSuccessful)
            {
                var mappedCoupon = _mapper.Map<AppliedCouponModel>(response.Data);
                return Ok(mappedCoupon);
            }
            else
            {
                return BadRequest(response.ErrorMessage);
            }
        }

        [HttpPost]
        public IActionResult ActivateCoupons([FromBody] List<string> activatedCoupons)
        {
            if (activatedCoupons.IsNullOrEmpty())
            {
                return Ok();
            }
            var request = _couponService.UseCoupon(activatedCoupons);
            return Ok();
        }
    }
}
