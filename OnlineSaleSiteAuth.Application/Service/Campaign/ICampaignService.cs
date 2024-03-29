﻿using OnlineSaleSiteAuth.Application.Service.Campaign.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineSaleSiteAuth.Application.Service.Campaign
{
    public interface ICampaignService
    {
        Task<ServiceResponse> AddCampaign(AddCampaignDto request);
    }
}
