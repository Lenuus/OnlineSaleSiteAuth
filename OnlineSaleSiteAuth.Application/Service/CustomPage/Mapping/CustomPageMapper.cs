using AutoMapper;
using OnlineSaleSiteAuth.Application.Service.CustomPage.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineSaleSiteAuth.Application.Service.CustomPage.Mapping
{
    public class CustomPageMapper : Profile
    {
        public CustomPageMapper()
        {
            CreateMap<CreateCustomPageDto, Domain.CustomPage>();
        }

    }
}
