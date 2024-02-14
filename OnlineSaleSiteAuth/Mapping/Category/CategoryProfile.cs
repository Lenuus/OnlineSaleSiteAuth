using AutoMapper;
using OnlineSaleSiteAuth.Application.Service.Category.Dtos;
using OnlineSaleSiteAuth.Models.Category;

namespace OnlineSaleSiteAuth.Mapping.Category
{
    public class CategoryProfile:Profile
    {
       public CategoryProfile()
        {
            CreateMap<AddCategoryModel, AddCategoryDto>();
        }
    }
}
