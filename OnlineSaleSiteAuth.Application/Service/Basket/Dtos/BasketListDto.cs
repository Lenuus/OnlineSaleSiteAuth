using OnlineSaleSiteAuth.Application.Service.Product.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineSaleSiteAuth.Application.Service.Basket.Dtos
{
    public class BasketListDto
    {
        public Guid ProductId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal TotalPrice { get; set; }
        public List<ProductListImageDto> Images { get; set; } = new List<ProductListImageDto>();
        public List<BasketListCampaignDto> Campaigns { get; set; }

    }
}
