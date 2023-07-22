using MyCompany.NewProject.Application.Abstractions.Messaging;
using MyCompany.NewProject.Core.Model.Dictionaries;
using MyCompany.NewProject.Core.Results;
using MyCompany.NewProject.Core.Results.Errors;
using MyCompany.NewProject.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace MyCompany.NewProject.Application.Features.Dictionaries;

public interface IDictionaryDeleteCommand : ICommand
{
    string Id { get; }
}

internal abstract class DictionaryDeleteCommandHandler<TCommand, TDictionary> : IRequestHandler<TCommand, Result>
    where TCommand : IDictionaryDeleteCommand
    where TDictionary : Dictionary
{
    private readonly ApplicationDbContext _db;

    public DictionaryDeleteCommandHandler(ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<Result> Handle(TCommand command, CancellationToken cancellationToken)
    {
        var dictionary = await _db.Set<TDictionary>().SingleOrDefaultAsync(x => x.Id == command.Id, cancellationToken);

        if (dictionary is null)
        {
            return new ValidationError("Dictionary is not found.");
        }

        dictionary.Deleted = true;
        _db.Update(dictionary);
        await _db.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}