using OnlineSaleSiteAuth.Application.Service.Coupon.Dtos;
using OnlineSaleSiteAuth.Application.Service.Product.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineSaleSiteAuth.Application.Service.Coupon
{
    public interface ICouponService
    {

        Task<ServiceResponse> AddCoupon(AddCouponRequestDto request);
        Task<ServiceResponse<AppliedCouponDto>> ApplyCoupon(string Coupon);
        Task<ServiceResponse>CheckCoupon(string Coupon);
        Task<ServiceResponse> UseCoupon(List<string> Coupons);
    }
}
/*
 * kupon girildi
controllera kupon gitti
giden kupon servise gönderildi var mı yok mu date i geçerli mi used mı unused mı kontrol etti
hepsi okeylendiyse
kontrollera geri
düşürülmüş fiyatı gönderdi
controllerda bu fiyatı eskisi yerine uyguladı
 * 
 */