using System.ComponentModel.DataAnnotations;

namespace OnlineSaleSiteAuth.Models.Coupon
{
    public class AddCouponRequestModel
    {
        [Required]
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
