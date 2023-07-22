using MyCompany.NewProject.Application.Abstractions.Messaging;
using MyCompany.NewProject.Core.Results;
using MyCompany.NewProject.Integration.MicrosoftGraph.Users;
using FluentValidation;

namespace MyCompany.NewProject.Application.Features.Users;

public sealed record MicrosoftGraphUserResponse(string Email, string DisplayName);

public sealed record GetUserFromMicrosoftGraphByEmailQuery(string Email) : IQuery<MicrosoftGraphUserResponse>;

public sealed class GetUserFromMicrosoftGraphByEmailQueryValidator : AbstractValidator<GetUserFromMicrosoftGraphByEmailQuery>
{
    public GetUserFromMicrosoftGraphByEmailQueryValidator()
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
    }
}

internal sealed record GetUserFromMicrosoftGraphByEmailQueryHandler : IQueryHandler<GetUserFromMicrosoftGraphByEmailQuery, MicrosoftGraphUserResponse>
{
    private readonly IMicrosoftGraphUserService _microsoftGraphUserService;

    public GetUserFromMicrosoftGraphByEmailQueryHandler(IMicrosoftGraphUserService microsoftGraphUserService)
    {
        _microsoftGraphUserService = microsoftGraphUserService;
    }

    public async Task<Result<MicrosoftGraphUserResponse>> Handle(GetUserFromMicrosoftGraphByEmailQuery request, CancellationToken cancellationToken)
    {
        var result = await _microsoftGraphUserService.GetByEmailAsync(request.Email);
        if (result.IsFailure)
        {
            return result.Error;
        }
        return new MicrosoftGraphUserResponse(result.Value.Email, result.Value.DisplayName);
    }
}
