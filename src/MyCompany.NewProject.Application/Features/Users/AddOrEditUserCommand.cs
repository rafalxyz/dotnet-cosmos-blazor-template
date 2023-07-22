using MyCompany.NewProject.Application.Abstractions;
using MyCompany.NewProject.Application.Abstractions.Messaging;
using MyCompany.NewProject.Core.Model.Users;
using MyCompany.NewProject.Core.Results;
using MyCompany.NewProject.Core.Results.Errors;
using MyCompany.NewProject.Integration.MicrosoftGraph.Users;
using MyCompany.NewProject.Persistence;
using MyCompany.NewProject.Persistence.Extensions;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace MyCompany.NewProject.Application.Features.Users;

public sealed class AddOrEditUserCommand : ICommand<ResourceId>
{
    public required string? Id { get; set; }
    public required string Email { get; set; }
}

public sealed class AddOrEditUserCommandValidator : AbstractValidator<AddOrEditUserCommand>
{
    public AddOrEditUserCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email must not be empty.")
            .EmailAddress().WithMessage("Email must be valid.");
    }
}

internal sealed class AddOrEditUserCommandHandler : ICommandHandler<AddOrEditUserCommand, ResourceId>
{
    private readonly ApplicationDbContext _db;
    private readonly IMicrosoftGraphUserService _microsoftGraphUserService;

    public AddOrEditUserCommandHandler(ApplicationDbContext db, IMicrosoftGraphUserService microsoftGraphUserService)
    {
        _db = db;
        _microsoftGraphUserService = microsoftGraphUserService;
    }

    public Task<Result<ResourceId>> Handle(AddOrEditUserCommand command, CancellationToken cancellationToken)
    {
        return command.Id is null
            ? AddUser(command, cancellationToken)
            : EditUser(command, cancellationToken);
    }

    private async Task<Result<ResourceId>> AddUser(AddOrEditUserCommand command, CancellationToken cancellationToken)
    {
        var userWithEmail = await _db.Users.GetByEmailAsync(command.Email, cancellationToken);
        if (userWithEmail is not null)
        {
            return new ValidationError("User with the same email already exists.");
        }

        var microsoftGraphUserResult = await _microsoftGraphUserService.GetByEmailAsync(command.Email);
        if (microsoftGraphUserResult.IsFailure)
        {
            return new ValidationError(microsoftGraphUserResult.Error.Message);
        }

        var user = User.Create(
            displayName: microsoftGraphUserResult.Value.DisplayName,
            azureActiveDirectoryUserId: microsoftGraphUserResult.Value.Id,
            email: command.Email);

        _db.Add(user);
        await _db.SaveChangesAsync(cancellationToken);

        return new ResourceId(user.Id);
    }

    private async Task<Result<ResourceId>> EditUser(AddOrEditUserCommand command, CancellationToken cancellationToken)
    {
        var userWithEmail = await _db.Users.GetByEmailAsync(command.Email, cancellationToken);
        if (userWithEmail is not null && userWithEmail.Id != command.Id)
        {
            return new ValidationError("User with the same email already exists.");
        }

        var user = await _db.Users.SingleOrDefaultAsync(x => x.Id == command.Id, cancellationToken);
        if (user is null)
        {
            return new ValidationError("User not found.");
        }

        var microsoftGraphUserResult = await _microsoftGraphUserService.GetByEmailAsync(command.Email);
        if (microsoftGraphUserResult.IsFailure)
        {
            return new ValidationError(microsoftGraphUserResult.Error.Message);
        }

        user.Update(
            displayName: microsoftGraphUserResult.Value.DisplayName,
            azureActiveDirectoryUserId: microsoftGraphUserResult.Value.Id,
            email: command.Email);

        _db.Update(user);
        await _db.SaveChangesAsync(cancellationToken);

        return new ResourceId(user.Id);
    }
}