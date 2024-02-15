using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineSaleSiteAuth.Domain
{
    public class ProductCampaign : IBaseEntity, ISoftDeletable
    {
        public Guid Id { get; set; }
        [Required]
        public Guid CampaignId { get; set; }

        public Campaign Campaign { get; set; }
        [Required]
        public Guid ProductId { get; set; }

        public Product Product { get; set; }
        public decimal DiscountedPrice { get; set; }

        public bool IsDeleted { get; set; }
    }
}
