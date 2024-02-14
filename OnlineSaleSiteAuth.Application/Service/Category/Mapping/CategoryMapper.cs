using AutoMapper;
using OnlineSaleSiteAuth.Application.Service.Category.Dtos;

namespace OnlineSaleSiteAuth.Application.Service.Category.Mapping
{
    public class CategoryMapper : Profile
    {
        public CategoryMapper()
        {
            CreateMap<AddCategoryDto, Domain.Category>();
        }
    }
}
