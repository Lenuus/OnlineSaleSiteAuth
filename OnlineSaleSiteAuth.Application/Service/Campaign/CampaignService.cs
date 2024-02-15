using AutoMapper;
using OnlineSaleSiteAuth.Application.Service.File;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineSaleSiteAuth.Application.Service.Campaign
{
    public class CampaignService: ICampaignService
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
    }
}
