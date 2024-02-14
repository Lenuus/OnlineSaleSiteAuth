using OnlineSaleSiteAuth.Application.Service.Product.Dtos;
using OnlineSaleSiteAuth.Domain;
using System.ComponentModel.DataAnnotations;

namespace OnlineSaleSiteAuth.Models.Product
{
    public class UpdateProductRequestModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public int Stock { get; set; }

        public List<Guid> SelectedCategories { get; set; } = new List<Guid>();

        public List<UpdateProductRequestCategoryModel> Categories { get; set; } = new List<UpdateProductRequestCategoryModel>();

        public List<UpdateProductRequestImageModel> Images { get; set; } = new List<UpdateProductRequestImageModel>();

        public List<IFormFile> NewImages { get; set; } = new List<IFormFile>();
    }

}
