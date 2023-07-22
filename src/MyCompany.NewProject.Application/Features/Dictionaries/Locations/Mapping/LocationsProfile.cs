using AutoMapper;
using MyCompany.NewProject.Core.Model.Dictionaries;

namespace MyCompany.NewProject.Application.Features.Dictionaries.Locations.Mapping;

internal sealed class LocationsProfile : Profile
{
    public LocationsProfile()
    {
        CreateMap<Location, LocationDto>();
        CreateMap<UpsertLocationCommand, Location>();
    }
}
