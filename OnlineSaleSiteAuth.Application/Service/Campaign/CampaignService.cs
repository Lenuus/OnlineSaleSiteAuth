using AutoMapper;
using OnlineSaleSiteAuth.Application.Service.Campaign.Dtos;
using OnlineSaleSiteAuth.Application.Service.File;
using OnlineSaleSiteAuth.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineSaleSiteAuth.Application.Service.Campaign
{
    public class CampaignService : ICampaignService
    {
        private readonly IRepository<Domain.Product> _productRepository;
        private readonly IMapper _mapper;
        private readonly IRepository<Domain.Campaign> _campaignRepository;

        public CampaignService(IRepository<Domain.Campaign> campaignRepository, IMapper mapper, IRepository<Domain.Product> productRepository)
        {
            _campaignRepository = campaignRepository;
            _mapper = mapper;
            _productRepository = productRepository;
        }

        public async Task<ServiceResponse> AddCampaign(AddCampaignDto request)
        {
            if (request == null)
            {
                return new ServiceResponse(false, "Not Found");
            }
            if (request.StartDate > request.EndDate || request.EndDate < DateTime.UtcNow)
            {
                return new ServiceResponse(false, "Dates are not valid");
            }
            var entityMapped = _mapper.Map<Domain.Campaign>(request);
            entityMapped.Products.AddRange(request.Products.Select(p => new ProductCampaign { ProductId = p }));
            await _campaignRepository.Create(entityMapped);
            return new ServiceResponse();
        }
    }
}
