using OnlineSaleSiteAuth.Application.Service.Product.Dtos;
using OnlineSaleSiteAuth.Domain;
using System.ComponentModel.DataAnnotations;

namespace OnlineSaleSiteAuth.Models.Product
{
    public class AddProductRequestModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public decimal Price { get; set; }
       
        [Required]
        public int Stock { get; set; }

        [Required]
        public ICollection<Guid> Categories { get; set; }

        [Required]
        public IEnumerable<IFormFile> Images { get; set; }

    }
}
