using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineSaleSiteAuth.Application.Service.Coupon.Dtos
{
    public class AddCouponRequestDto
    {
        public Guid ProductId { get; set; }

        [Required]
        public string CouponCode { get; set; }

        public decimal DiscountRate { get; set; }

        public decimal DecreasedPrice { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public bool Used { get; set; }
    }
}
