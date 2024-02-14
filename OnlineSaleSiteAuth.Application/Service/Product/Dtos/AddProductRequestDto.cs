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
    public class AddProductRequestDto
    {
        public string Name { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        public int Stock { get; set; }

        public List<IFormFile> Images { get; set; } = new List<IFormFile>();

        [Required]
        public List<Guid> Categories { get; set; } = new List<Guid>();
    }
}
