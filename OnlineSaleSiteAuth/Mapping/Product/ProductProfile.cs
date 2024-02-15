using AutoMapper;
using OnlineSaleSiteAuth.Application.Service.Product.Dtos;
using OnlineSaleSiteAuth.Common.Dtos;
using OnlineSaleSiteAuth.Models;
using OnlineSaleSiteAuth.Models.Product;

namespace OnlineSaleSiteAuth.Mapping.Show
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<GetAllProductRequestModel, GetAllProductRequestDto>();

            CreateMap<ProductListDto, ProductListModel>();

            CreateMap<PagedResponseDto<ProductListDto>, PagedResponseModel<ProductListModel>>();

            CreateMap<AddProductRequestModel, AddProductRequestDto>();

            CreateMap<ProductListDto, UpdateProductRequestModel>();

            CreateMap<UpdateProductRequestModel, UpdateProductRequestDto>();

            CreateMap<UpdateProductRequestImageModel, UpdateProductRequestImageDto>();

            CreateMap<ProductListImageDto, ProductListImageModel>();

            CreateMap<ProductListCategoryDto, ProductListCategoryModel>();

            CreateMap<ProductListCategoryDto, UpdateProductRequestCategoryModel>();

            CreateMap<ProductListImageDto, UpdateProductRequestImageModel>();

            CreateMap<ProductListCampaignDto, ProductListCampaignModel>();

        }
    }
}
