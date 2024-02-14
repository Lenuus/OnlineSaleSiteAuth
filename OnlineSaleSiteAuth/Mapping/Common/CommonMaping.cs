using AutoMapper;
using OnlineSaleSiteAuth.Common.Dtos;
using OnlineSaleSiteAuth.Models;

namespace OnlineSaleSiteAuth.Mapping.Common
{
    public class CommonMapping : Profile
    {

        public CommonMapping()
        {
            CreateMap<PagedRequestModel, PagedRequestDto>();
        }

    }
}
