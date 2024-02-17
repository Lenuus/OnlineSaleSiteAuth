using AutoMapper;
using OnlineSaleSiteAuth.Application.Service.Campaign.Dtos;
using OnlineSaleSiteAuth.Application.Service.Product.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineSaleSiteAuth.Application.Service.Campaign.Mapping
{
    public class CampaignMapper:Profile
    {
        public CampaignMapper() 
        {
            CreateMap<AddCampaignDto, Domain.Campaign>().ForMember(dest => dest.Products, opt => opt.Ignore());

            
        } 
    }
}
