using AutoMapper;
using OnlineSaleSiteAuth.Application.Service.Basket.Dtos;
using OnlineSaleSiteAuth.Application.Service.Product.Dtos;
using OnlineSaleSiteAuth.Common.Dtos;
using OnlineSaleSiteAuth.Models;
using OnlineSaleSiteAuth.Models.Basket;
using OnlineSaleSiteAuth.Models.Product;

namespace OnlineSaleSiteAuth.Mapping.Basket
{
    public class BasketProfile : Profile
    {
        public BasketProfile()
        {
            CreateMap<BasketListDto, BasketListModel>();
            CreateMap<ProductListImageDto, ProductListImageModel>();
            CreateMap<BasketListSessionModel, BasketListSessionDto>();
        }
    }
}
