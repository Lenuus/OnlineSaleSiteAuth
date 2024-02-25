using OnlineSaleSiteAuth.Application.Service.CustomPage.Dtos;
using OnlineSaleSiteAuth.Application.Service.Product.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineSaleSiteAuth.Application.Service.CustomPage
{
    public interface ICustomPageService
    {
        Task<ServiceResponse> CreateCustomPage(CreateCustomPageDto request);

        Task<ServiceResponse<List<CustomPageDetailDto>>> GetAllCustomPage();

        Task<ServiceResponse<CustomPageDetailDto>> GetCustomPage(Guid id);

    }
}
