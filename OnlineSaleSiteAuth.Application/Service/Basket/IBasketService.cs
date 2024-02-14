using OnlineSaleSiteAuth.Application.Service.Basket.Dtos;
using OnlineSaleSiteAuth.Application.Service.Product.Dtos;
using OnlineSaleSiteAuth.Common.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineSaleSiteAuth.Application.Service.Basket
{
    public interface IBasketService
    {
        Task<ServiceResponse> AddToBasket(Guid productId, int quantity = 1);
        Task<ServiceResponse> RemoveFromBasket(Guid productId, int quantity = 1);
        Task<ServiceResponse<List<BasketListDto>>> GetAllBasket();
        Task<ServiceResponse> ClearBasket();
    }
}
