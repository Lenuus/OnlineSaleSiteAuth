using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineSaleSiteAuth.Application.Service.Basket.Dtos
{
    public class BasketListSessionDto
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
