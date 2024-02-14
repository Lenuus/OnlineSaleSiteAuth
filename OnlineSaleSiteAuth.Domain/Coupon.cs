using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineSaleSiteAuth.Domain
{
    public class Coupon : IBaseEntity, ISoftDeletable
    {
        public Guid Id { get; set; }
        [Required]
        public Guid ProductId { get; set; }

        public Product Product { get; set; }
        [Required]
        public string CouponCode { get; set; }

        public decimal DiscountRate { get; set; }

        public decimal DecreasedPrice { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public bool Used { get; set; }

        public bool IsDeleted { get; set; }

    }
}
