using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineSaleSiteAuth.Application.Service.Product.Dtos
{
   public class UpdateProductRequestImageDto
    {
        public Guid Id { get; set; }

        public string Path { get; set; }
    }
}
