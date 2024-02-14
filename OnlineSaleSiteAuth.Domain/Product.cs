using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineSaleSiteAuth.Domain
{
    public class Product : IBaseEntity, ISoftDeletable
    {
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Required]
        public int Stock { get; set; }
        public bool IsDeleted { get; set; }
        public List<ProductCategory> Categories { get; set; } = new List<ProductCategory>();
        public List<OrderDetail> OrderDetails { get; set; }
        public List<Image> Images { get; set; } = new List<Image>();
        public List<Coupon> Coupons { get; set; }= new List<Coupon>();
    }

}
