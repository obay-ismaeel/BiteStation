using AutoMapper;
using BiteStation.Domain.Dtos;
using BiteStation.Domain.Models;

namespace BiteStation.Presentation.Profiles;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Restaurant, OutgoingRestaurantDto>();
        CreateMap<IncomingRestaurantDto, Restaurant>();

        CreateMap<MenuDto, Menu>();
        CreateMap<Menu, MenuDto>();

        CreateMap<IncomingItemDto, Item>();
        CreateMap<Item, OutgoingItemDto>();
    }
}
