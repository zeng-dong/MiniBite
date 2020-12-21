using AutoMapper;
using MiniBite.Api.Purchasing.Entities;

namespace MiniBite.Api.Purchasing.ApiModels.Mapping
{
    public class InventoryMappingProfile : Profile
    {
        public InventoryMappingProfile()
        {
            CreateMap<InventoryProductDto, Item>()
                .ForMember(d => d.UnitCost, opt => opt.MapFrom(s => s.Price));
        }
    }
}
