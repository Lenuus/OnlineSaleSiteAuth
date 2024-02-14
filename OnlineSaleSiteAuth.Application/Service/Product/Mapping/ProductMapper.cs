using AutoMapper;
using Microsoft.AspNetCore.Http;
using OnlineSaleSiteAuth.Application.Service.Product.Dtos;
using OnlineSaleSiteAuth.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineSaleSiteAuth.Application.Service.Product.Mapping
{
    public class ProductMapper:Profile
    {
        public ProductMapper() 
        {
            CreateMap<AddProductRequestDto, Domain.Product>()
                .ForMember(dest => dest.Categories, opt => opt.Ignore())
                .ForMember(dest => dest.Images, opt => opt.Ignore());
            CreateMap<Guid, Domain.Product>();
            CreateMap<Domain.Product, ProductListDto>();
        }
    }
}
