using AutoMapper;
using MyCompany.NewProject.Core.Abstractions.Cache;
using MyCompany.NewProject.Core.Model.Dictionaries;

namespace MyCompany.NewProject.Application.Features.Dictionaries.Locations;

public sealed record GetAllLocationsQuery : IDictionaryQuery<LocationDto>;

public sealed record LocationDto(string Id, string Name, string CountryId, string CountryName);

internal sealed class GetAllLocationsQueryHandler : DictionaryQueryHandler<GetAllLocationsQuery, Location, LocationDto>
{
    public GetAllLocationsQueryHandler(IMapper mapper, IEntityCache entityCache) : base(mapper, entityCache) { }
}
