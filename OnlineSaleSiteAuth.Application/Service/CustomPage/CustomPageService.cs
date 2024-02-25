using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OnlineSaleSiteAuth.Application.Service.CustomPage.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace OnlineSaleSiteAuth.Application.Service.CustomPage
{
    public class CustomPageService : ICustomPageService
    {
        private readonly IRepository<Domain.CustomPage> _customPage;
        private readonly IMapper _mapper;

        public CustomPageService(IMapper mapper, IRepository<Domain.CustomPage> customPage)
        {
            _mapper = mapper;
            _customPage = customPage;
        }

        public async Task<ServiceResponse> CreateCustomPage(CreateCustomPageDto request)
        {
            var entity = _mapper.Map<Domain.CustomPage>(request);
            if (entity != null)
            {
                await _customPage.Create(entity);
                return new ServiceResponse();
            }
            return new ServiceResponse(false, "Process Failed");
        }

        public async Task<ServiceResponse<List<CustomPageDetailDto>>> GetAllCustomPage()
        {
            var query = _customPage.GetAll().Where(f => !f.IsDeleted).Select(f => new CustomPageDetailDto
            {
                Id = f.Id,
                Title = f.Title,
                HtmlContent = f.HtmlContent,
            }).ToList();
            if (query != null)
            {
                return new ServiceResponse<List<CustomPageDetailDto>>(query);
            }
            return new ServiceResponse<List<CustomPageDetailDto>>(null, false, "Not Found");
        }

        public async Task<ServiceResponse<CustomPageDetailDto>> GetCustomPage(Guid id)
        {
            var customPage = await _customPage.GetAll().FirstOrDefaultAsync(f => !f.IsDeleted && f.Id == id).ConfigureAwait(false);

            if (customPage != null)
            {
                var customPageDetailDto = new CustomPageDetailDto { Id = customPage.Id, Title = customPage.Title, HtmlContent = customPage.HtmlContent };
                return new ServiceResponse<CustomPageDetailDto>(customPageDetailDto);
            }
            else
            {
                return new ServiceResponse<CustomPageDetailDto>(null, false, "Not Found");
            }
        }

    }
}
