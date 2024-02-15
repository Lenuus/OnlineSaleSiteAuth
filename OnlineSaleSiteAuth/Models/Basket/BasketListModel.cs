using OnlineSaleSiteAuth.Application.Service.Product.Dtos;
using OnlineSaleSiteAuth.Models.Product;

namespace OnlineSaleSiteAuth.Models.Basket
{
    public class BasketListModel
    {
        public Guid ProductId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal TotalPrice { get; set; }
        public List<ProductListImageModel> Images { get; set; } = new List<ProductListImageModel>();
        public List<BasketListCampaignModel> Campaigns { get; set; }

    }

}
