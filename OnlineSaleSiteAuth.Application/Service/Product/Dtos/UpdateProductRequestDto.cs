using Microsoft.AspNetCore.Http;
using OnlineSaleSiteAuth.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineSaleSiteAuth.Application.Service.Product.Dtos
{
    public class UpdateProductRequestDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public int Stock { get; set; }

        public List<Guid> SelectedCategories { get; set; } = new List<Guid>();

        public List<UpdateProductRequestCategoryDto> Categories { get; set; } = new List<UpdateProductRequestCategoryDto>();

        public List<UpdateProductRequestImageDto> Images { get; set; } = new List<UpdateProductRequestImageDto>();

        public List<IFormFile> NewImages { get; set; } = new List<IFormFile>();
    }
}
