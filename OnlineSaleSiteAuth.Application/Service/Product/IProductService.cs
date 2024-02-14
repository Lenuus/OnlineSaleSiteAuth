using OnlineSaleSiteAuth.Application.Service.Category.Dtos;
using OnlineSaleSiteAuth.Application.Service.Product.Dtos;
using OnlineSaleSiteAuth.Common.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineSaleSiteAuth.Application.Service.Product
{
    public interface IProductService
    {
        Task<ServiceResponse> DeleteProduct(Guid id);

        Task<ServiceResponse<ProductListDto>> GetProductById(Guid id);

        Task<ServiceResponse> AddProduct(AddProductRequestDto request);

        Task<ServiceResponse> UpdateProduct(UpdateProductRequestDto request);

        Task<ServiceResponse<PagedResponseDto<ProductListDto>>> GetAllProducts(GetAllProductRequestDto request);

        Task<ServiceResponse<List<ProductListDto>>> GetAllProductIds();

        Task<ServiceResponse> DeleteImage(Guid deleteImageId);
    }
}
