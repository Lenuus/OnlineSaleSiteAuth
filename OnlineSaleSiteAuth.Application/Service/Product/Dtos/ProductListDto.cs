using OnlineSaleSiteAuth.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineSaleSiteAuth.Application.Service.Product.Dtos
{
    public class ProductListDto
    {
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        public int Stock { get; set; }

        public List<ProductListCategoryDto> Categories { get; set; } = new List<ProductListCategoryDto>();

        public List<ProductListImageDto> Images { get; set; } = new List<ProductListImageDto>();

    }
}
