using MyCompany.NewProject.Application.Abstractions.Messaging;
using MyCompany.NewProject.Core.Abstractions.Cache;
using MyCompany.NewProject.Core.Model.Users;
using MyCompany.NewProject.Core.Results;

namespace MyCompany.NewProject.Application.Features.Users;

public sealed record GetUsersQuery : IQuery<IReadOnlyList<UserDto>>;

public sealed record UserDto(
    string Id,
    string DisplayName,
    string Email
);

internal sealed class GetUsersQueryHandler : IQueryHandler<GetUsersQuery, IReadOnlyList<UserDto>>
{
    private readonly IEntityCache _cache;

    public GetUsersQueryHandler(IEntityCache cache)
    {
        _cache = cache;
    }

    public async Task<Result<IReadOnlyList<UserDto>>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
        var users = await _cache.GetAll<User>();

        return users.OrderBy(x => x.DisplayName)
            .Select(x => new UserDto(
                Id: x.Id,
                DisplayName: x.DisplayName,
                Email: x.Email
            )).ToList();
    }
}