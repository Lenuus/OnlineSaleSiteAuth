using AutoMapper;
using OnlineSaleSiteAuth.Application;
using OnlineSaleSiteAuth.Application.Service.Coupon.Dtos;
using OnlineSaleSiteAuth.Models.Coupon;

namespace OnlineSaleSiteAuth.Mapping.Coupon
{
    public class CouponProfile:Profile
    {
        public CouponProfile()
        {
            CreateMap<AddCouponRequestModel, AddCouponRequestDto>();
            CreateMap<AppliedCouponDto,AppliedCouponModel>();

        }
    }
}
