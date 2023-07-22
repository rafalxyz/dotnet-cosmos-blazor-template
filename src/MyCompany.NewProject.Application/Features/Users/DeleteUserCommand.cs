using MyCompany.NewProject.Application.Abstractions.Messaging;
using MyCompany.NewProject.Core.Results;
using MyCompany.NewProject.Core.Results.Errors;
using MyCompany.NewProject.Persistence;
using Microsoft.EntityFrameworkCore;

namespace MyCompany.NewProject.Application.Features.Users;

public sealed class DeleteUserCommand : ICommand
{
    public required string UserId { get; init; }
}

internal sealed class DeleteUserCommandHandler : ICommandHandler<DeleteUserCommand>
{
    private readonly ApplicationDbContext _db;

    public DeleteUserCommandHandler(ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<Result> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _db.Users.SingleOrDefaultAsync(x => x.Id == request.UserId, cancellationToken);
        if (user is null)
        {
            return new ValidationError($"User with ID {request.UserId} is not found.");
        }

        user.Delete();
        await _db.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
