using AutoMapper;
using OnlineSaleSiteAuth.Application.Service.Campaign.Dtos;
using OnlineSaleSiteAuth.Models.Campaign;

namespace OnlineSaleSiteAuth.Mapping.Campaign
{
    public class CampaignProfile : Profile
    {
        public CampaignProfile()
        {
            CreateMap<AddCampaignModel, AddCampaignDto>();
        }
    }
}
