using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using OnlineSaleSiteAuth.Application.Service.Basket.Dtos;
using OnlineSaleSiteAuth.Application.Service.Claim;
using OnlineSaleSiteAuth.Application.Service.Coupon.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace OnlineSaleSiteAuth.Application.Service.Coupon
{
    public class CouponService : ICouponService
    {
        private readonly IHttpContextAccessor _httpContext;
        private readonly IMapper _mapper;
        private readonly IClaimManager _claimManager;
        private readonly IRepository<Domain.Product> _productRepository;
        private readonly IRepository<Domain.Coupon> _couponRepository;

        public CouponService(IRepository<Domain.Product> productRepository, IClaimManager claimManager, IMapper mapper, IHttpContextAccessor httpContext, IRepository<Domain.Coupon> couponRepository)
        {
            _productRepository = productRepository;
            _claimManager = claimManager;
            _mapper = mapper;
            _httpContext = httpContext;
            _couponRepository = couponRepository;
        }

        public async Task<ServiceResponse> AddCoupon(AddCouponRequestDto request)
        {
            var existedCoupon = _couponRepository.GetAll().FirstOrDefault(f => f.CouponCode == request.CouponCode);
            if (existedCoupon != null)
            {
                return new ServiceResponse(false, "This code is already exist");
            }

            if (request.StartDate >= request.EndDate || request.EndDate < DateTime.UtcNow)
            {
                return new ServiceResponse(false, "Dates are not available");
            }

            var mappedRequest = _mapper.Map<Domain.Coupon>(request);
            await _couponRepository.Create(mappedRequest).ConfigureAwait(false);

            var product = await _productRepository.GetById(request.ProductId);
            if (product != null)
            {
                product.Coupons.Add(mappedRequest);
                await _productRepository.Update(product).ConfigureAwait(false);
            }
            else
            {
                return new ServiceResponse(false, "Product not found");
            }
            return new ServiceResponse();
        }


        public async Task<ServiceResponse<AppliedCouponDto>> ApplyCoupon(string couponCode)
        {
            var coupon = _couponRepository.GetAll()
                .FirstOrDefault(f => !f.IsDeleted && !f.Used && f.CouponCode == couponCode && f.EndDate > DateTime.UtcNow && f.StartDate <= DateTime.UtcNow);

            if (coupon != null)
            {
                var productId = coupon.ProductId;
                var product = await _productRepository.GetById(productId);

                var userId = _httpContext.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var basketKey = $"Basket_{userId}";
                byte[] basket = null;
                if (!_httpContext.HttpContext.Session.TryGetValue(basketKey, out basket))
                {
                    return new ServiceResponse<AppliedCouponDto>(null, false, "Basket not found in session");
                }

                var basketJson = Encoding.UTF8.GetString(basket);
                var basketItems = JsonConvert.DeserializeObject<List<BasketListSessionDto>>(basketJson);
                var itemQuantity = basketItems.FirstOrDefault(f => f.ProductId == productId).Quantity;
                if (product != null)
                {
                    var itemPrice = product.Price * itemQuantity;
                    var discountRate = coupon.DiscountRate;
                    var newPrice = itemPrice - ((itemPrice * discountRate) / 100);

                    var appliedCoupon = new AppliedCouponDto
                    {
                        ProductId = productId,
                        OriginalPrice = itemPrice,
                        DiscountRate = discountRate,
                        DiscountedPrice = newPrice
                    };
                    return new ServiceResponse<AppliedCouponDto>(appliedCoupon);
                }
                else
                {
                    return new ServiceResponse<AppliedCouponDto>(null, false, "Not Found");
                }
            }
            else
            {
                return new ServiceResponse<AppliedCouponDto>(null, false, "Not Found or Expired");
            }
        }
        public async Task<ServiceResponse> CheckCoupon(string Coupon)
        {
            var existedCoupon = _couponRepository.GetAll().FirstOrDefault(f => f.CouponCode == Coupon);
            if (existedCoupon != null)
            {
                return new ServiceResponse(false, "Already Exists");
            }
            return new ServiceResponse();
        }
        public async Task<ServiceResponse> UseCoupon(List<string> Coupons)
        {
            var usedCoupons = _couponRepository.GetAll().Where(c => Coupons.Contains(c.CouponCode)).ToList();
            usedCoupons.ForEach(c => c.Used = true);

            foreach (var coupon in usedCoupons)
            {
                await _couponRepository.Update(coupon).ConfigureAwait(false);
            }
            return new ServiceResponse();
        }

    }
}
