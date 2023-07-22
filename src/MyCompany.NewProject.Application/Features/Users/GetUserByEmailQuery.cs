using MyCompany.NewProject.Application.Abstractions.Messaging;
using MyCompany.NewProject.Core.Results;
using MyCompany.NewProject.Core.Results.Errors;
using MyCompany.NewProject.Persistence;
using MyCompany.NewProject.Persistence.Extensions;
using FluentValidation;

namespace MyCompany.NewProject.Application.Features.Users;

public sealed record GetUserByEmailResponse(string UserId);

public sealed record GetUserByEmailQuery(string Email) : IQuery<GetUserByEmailResponse>;

public sealed class GetUserByEmailValidator : AbstractValidator<GetUserByEmailQuery>
{
    public GetUserByEmailValidator()
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
    }
}

internal sealed record GetUserByEmailQueryHandler : IQueryHandler<GetUserByEmailQuery, GetUserByEmailResponse>
{
    private readonly ApplicationDbContext _db;

    public GetUserByEmailQueryHandler(ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<Result<GetUserByEmailResponse>> Handle(GetUserByEmailQuery request, CancellationToken cancellationToken)
    {
        var userWithEmail = await _db.Users.GetByEmailAsync(request.Email, cancellationToken);
        if (userWithEmail is null)
        {
            return new NotFoundError("User with the given email does not exist.");
        }
        return new GetUserByEmailResponse(userWithEmail.Id);
    }
}
