using OnlineSaleSiteAuth.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineSaleSiteAuth.Application.Service.Product.Dtos
{
    public class ProductListCampaignDto
    {
        public decimal DiscountRate { get; set; }
        public decimal DiscountedPrice { get; set; }
       
    }
}
