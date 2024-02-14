using AutoMapper;
using OnlineSaleSiteAuth.Application.Service.Coupon.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineSaleSiteAuth.Application.Service.Coupon.Mapping
{
    public class CouponMapper:Profile
    {
        public CouponMapper() 
        {
            CreateMap<AddCouponRequestDto, Domain.Coupon>();
        }
    }
}
