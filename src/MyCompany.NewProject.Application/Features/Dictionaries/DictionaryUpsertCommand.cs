using AutoMapper;
using MyCompany.NewProject.Application.Abstractions;
using MyCompany.NewProject.Application.Abstractions.Messaging;
using MyCompany.NewProject.Core.Model.Dictionaries;
using MyCompany.NewProject.Core.Results;
using MediatR;

namespace MyCompany.NewProject.Application.Features.Dictionaries;

public interface IDictionaryUpsertCommand : ICommand<ResourceId>
{
    public string? Id { get; }
}

public abstract class DictionaryUpsertCommandBase : IDictionaryUpsertCommand
{
    public required string? Id { get; init; }
}

internal abstract class DictionaryUpsertCommandHandler<TCommand, TDictionary> : IRequestHandler<TCommand, Result<ResourceId>>
    where TCommand : IDictionaryUpsertCommand
    where TDictionary : Dictionary, new()
{
    private readonly IMapper _mapper;
    private readonly IDictionaryService _dictionaryService;

    protected DictionaryUpsertCommandHandler(IMapper mapper, IDictionaryService dictionaryService)
    {
        _mapper = mapper;
        _dictionaryService = dictionaryService;
    }

    public async Task<Result<ResourceId>> Handle(TCommand command, CancellationToken cancellationToken)
    {
        return await _dictionaryService.Handle<IDictionaryUpsertCommand, TDictionary>(command, dictionary => _mapper.Map(command, dictionary), cancellationToken);
    }
}
