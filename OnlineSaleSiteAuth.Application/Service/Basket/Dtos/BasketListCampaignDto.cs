using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineSaleSiteAuth.Application.Service.Basket.Dtos
{
    public class BasketListCampaignDto
    {
        public decimal DiscountRate { get; set; }
        public decimal DiscountedPrice { get; set; }
        public decimal DiscountedTotalPrice { get; set; }
    }
}
