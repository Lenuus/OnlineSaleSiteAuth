using OnlineSaleSiteAuth.Application.Service.Product.Dtos;
using OnlineSaleSiteAuth.Domain;
using System.ComponentModel.DataAnnotations;

namespace OnlineSaleSiteAuth.Models.Product
{
    public class ProductListModel
    {
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        public int Stock { get; set; }

        public List<ProductListCategoryModel> Categories { get; set; } = new List<ProductListCategoryModel>();

        public List<ProductListImageModel> Images { get; set; } = new List<ProductListImageModel>();

        public List<ProductListCampaignModel> Campaigns { get; set; }

    }
}
