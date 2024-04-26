using AutoMapper;
using BiteStation.Domain.Models;
using BiteStation.Presentation.Dtos;

namespace BiteStation.Presentation.Profiles;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Restaurant, OutgoingRestaurantDto>();
        CreateMap<IncomingRestaurantDto, Restaurant>();
    }
}
