using AutoMapper;
using Common.Auth.Core.DTO;
using Common.Auth.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Auth.Service
{
    class DtoMapper : Profile
    {
        public DtoMapper()
        {
            CreateMap<Product, ProductDto>().ReverseMap();
            CreateMap<UserApp, UserAppDto>().ReverseMap();
        }

    }
}
