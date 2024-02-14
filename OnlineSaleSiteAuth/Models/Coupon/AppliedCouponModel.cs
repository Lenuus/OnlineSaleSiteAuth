namespace OnlineSaleSiteAuth.Models.Coupon
{
    public class AppliedCouponModel
    {
        public Guid ProductId { get; set; }

        public decimal OriginalPrice { get; set; }

        public decimal DiscountRate { get; set; }

        public decimal DiscountedPrice { get; set; }
    }
}
