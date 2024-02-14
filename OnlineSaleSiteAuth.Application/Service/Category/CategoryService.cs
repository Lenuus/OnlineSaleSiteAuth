using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OnlineSaleSiteAuth.Application.Service.Category.Dtos;
using OnlineSaleSiteAuth.Application.Service.Claim;

namespace OnlineSaleSiteAuth.Application.Service.Category
{
    public class CategoryService : ICategoryService
    {
        private readonly IRepository<Domain.Category> _categoryRepository;
        private readonly IMapper _mapper;
        private readonly IClaimManager _claimManager;

        public CategoryService(IRepository<Domain.Category> categoryRepository, IClaimManager claimManager, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _claimManager = claimManager;
            _mapper = mapper;
        }

        public async Task<ServiceResponse> AddCategory(AddCategoryDto request)
        {
            if (request == null)
            {
                return new ServiceResponse(false, "Uygun formatta değil");
            }

            var entity = _mapper.Map<Domain.Category>(request);
            await _categoryRepository.Create(entity).ConfigureAwait(false);
            return new ServiceResponse(true, string.Empty);
        }

        public async Task<ServiceResponse> DeleteCategory(Guid id)
        {
            var request = await _categoryRepository.GetById(id).ConfigureAwait(false);
            if (request == null)
            {
                return new ServiceResponse(false, "Oge bulunamadı");
            }

            await _categoryRepository.DeleteById(id).ConfigureAwait(false);
            return new ServiceResponse(true, string.Empty);
        }

        public async Task<ServiceResponse<List<CategoryListDto>>> GetAllCategory()
        {
            var existing = await _categoryRepository.GetAll()
                .Select(f => new CategoryListDto
                {
                    Id = f.Id,
                    Name = f.Name,
                })
                .ToListAsync()
                .ConfigureAwait(false);
            if (existing == null || !existing.Any())
            {
                return new ServiceResponse<List<CategoryListDto>>(null, false, "Kategoriler bulunamadı");
            }

            return new ServiceResponse<List<CategoryListDto>>(existing, true, string.Empty);
        }
    }
}
