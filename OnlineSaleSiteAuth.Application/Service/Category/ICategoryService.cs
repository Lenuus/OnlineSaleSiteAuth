using OnlineSaleSiteAuth.Application.Service.Category.Dtos;

namespace OnlineSaleSiteAuth.Application.Service.Category
{
    public interface ICategoryService
    {
        Task<ServiceResponse> DeleteCategory(Guid id);

        Task<ServiceResponse> AddCategory(AddCategoryDto request);

        Task<ServiceResponse<List<CategoryListDto>>> GetAllCategory();
    }
}
