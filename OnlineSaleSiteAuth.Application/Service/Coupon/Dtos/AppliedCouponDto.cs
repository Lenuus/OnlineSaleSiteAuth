using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineSaleSiteAuth.Application.Service.Coupon.Dtos
{
    public class AppliedCouponDto
    {
        public Guid ProductId { get; set; }

        public decimal OriginalPrice { get; set; }

        public decimal DiscountRate { get; set; }

        public decimal DiscountedPrice { get; set; }
    }
}
