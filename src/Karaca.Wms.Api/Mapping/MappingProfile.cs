using AutoMapper;
using Karaca.Wms.Api.Models;
using Karaca.Wms.Api.DTOs;

namespace Karaca.Wms.Api.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Product, ProductDto>().ReverseMap();
            CreateMap<Location, LocationDto>().ReverseMap();

        }
    }
}
