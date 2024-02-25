using AutoMapper;
using OnlineSaleSiteAuth.Application.Service.CustomPage.Dtos;


namespace OnlineSaleSiteAuth.Application.Service.CustomPage.Mapping
{
    public class CustomPageMapper : Profile
    {
        public CustomPageMapper()
        {
            CreateMap<CreateCustomPageDto, Domain.CustomPage>();
            CreateMap<Domain.CustomPage, CustomPageDetailDto>();
        }

    }
}
