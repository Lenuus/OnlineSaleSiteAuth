using AutoMapper;
using OnlineSaleSiteAuth.Application.Service.CustomPage.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace OnlineSaleSiteAuth.Application.Service.CustomPage
{
    public class CustomPage : ICustomPage
    {
        private readonly IRepository<Domain.CustomPage> _customPage;
        private readonly IMapper _mapper;

        public CustomPage(IMapper mapper, IRepository<Domain.CustomPage> customPage)
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

        public Task<ServiceResponse<CustomPageDetail>> GetAllCustomPage()
        {
            throw new NotImplementedException();
        }
    }
}
