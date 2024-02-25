using AutoMapper;
using OnlineSaleSiteAuth.Application.Service.CustomPage.Dtos;
using OnlineSaleSiteAuth.Models.CustomPage;

namespace OnlineSaleSiteAuth.Mapping.CustomPage
{
    public class CustomPageProfil : Profile
    {
        public CustomPageProfil()
        {
            CreateMap<CreateCustomPageModel, CreateCustomPageDto>();
            CreateMap<CustomPageDetailDto, CreateCustomPageModel>();
        }
    }
}
