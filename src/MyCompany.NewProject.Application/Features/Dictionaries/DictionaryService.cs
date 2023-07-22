using MyCompany.NewProject.Application.Abstractions;
using MyCompany.NewProject.Core.Model.Dictionaries;
using MyCompany.NewProject.Core.Results;
using MyCompany.NewProject.Core.Results.Errors;
using MyCompany.NewProject.Persistence;
using Microsoft.EntityFrameworkCore;

namespace MyCompany.NewProject.Application.Features.Dictionaries;

public interface IDictionaryService
{
    Task<Result<ResourceId>> Handle<TCommand, TDictionary>(TCommand command, Action<TDictionary> configure, CancellationToken cancellationToken = default)
        where TCommand : IDictionaryUpsertCommand
        where TDictionary : Dictionary, new();
}

internal sealed class DictionaryService : IDictionaryService
{
    private readonly ApplicationDbContext _db;

    public DictionaryService(ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<Result<ResourceId>> Handle<TCommand, TDictionary>(TCommand command, Action<TDictionary> configure, CancellationToken cancellationToken = default)
        where TCommand : IDictionaryUpsertCommand
        where TDictionary : Dictionary, new()
    {
        return command.Id is not null
            ? await Update(command, configure, cancellationToken)
            : await Create(configure, cancellationToken);
    }

    private async Task<Result<ResourceId>> Create<TDictionary>(Action<TDictionary> configure, CancellationToken cancellationToken)
        where TDictionary : Dictionary, new()
    {
        var dictionary = new TDictionary();
        configure(dictionary);
        dictionary.Id = Guid.NewGuid().ToString();

        var keyUniquenessResult = await ValidateKeyUniqueness(dictionary, cancellationToken);
        if (keyUniquenessResult.IsFailure)
        {
            return keyUniquenessResult.Error;
        }

        _db.Add(dictionary);
        await _db.SaveChangesAsync(cancellationToken);
        return new ResourceId(dictionary.Id);
    }

    private async Task<Result<ResourceId>> Update<TCommand, TDictionary>(TCommand command, Action<TDictionary> configure, CancellationToken cancellationToken)
        where TCommand : IDictionaryUpsertCommand
        where TDictionary : Dictionary
    {
        var dictionary = await _db.Set<TDictionary>().SingleOrDefaultAsync(x => x.Id == command.Id!, cancellationToken);
        if (dictionary is null)
        {
            return new ValidationError("Dictionary is not found.");
        }

        configure(dictionary);

        var keyUniquenessResult = await ValidateKeyUniqueness(dictionary, cancellationToken);
        if (keyUniquenessResult.IsFailure)
        {
            return keyUniquenessResult.Error;
        }

        _db.Update(dictionary);
        await _db.SaveChangesAsync(cancellationToken);
        return new ResourceId(dictionary.Id);
    }

    private async Task<Result> ValidateKeyUniqueness<TDictionary>(TDictionary dictionary, CancellationToken cancellationToken = default)
            where TDictionary : Dictionary
    {
        var dictionaryByKey = await _db.Set<TDictionary>().SingleOrDefaultAsync(x => x.Key == dictionary.CalculateKey(), cancellationToken);
        if (dictionaryByKey is not null && dictionaryByKey.Id != dictionary.Id)
        {
            return new ValidationError("Dictionary with the given key already exists.");
        }

        return Result.Success();
    }
}
