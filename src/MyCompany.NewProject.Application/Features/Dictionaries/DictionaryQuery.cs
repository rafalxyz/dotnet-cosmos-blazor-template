using AutoMapper;
using MyCompany.NewProject.Application.Abstractions.Messaging;
using MyCompany.NewProject.Core.Abstractions.Cache;
using MyCompany.NewProject.Core.Model.Dictionaries;
using MyCompany.NewProject.Core.Results;
using MediatR;

namespace MyCompany.NewProject.Application.Features.Dictionaries;

public interface IDictionaryQuery<TDto> : IQuery<IReadOnlyList<TDto>>
{
}

internal abstract class DictionaryQueryHandler<TQuery, TDictionary, TDto> : IRequestHandler<TQuery, Result<IReadOnlyList<TDto>>>
    where TQuery : IDictionaryQuery<TDto>
    where TDictionary : Dictionary
{
    private readonly IMapper _mapper;
    private readonly IEntityCache _entityCache;

    public DictionaryQueryHandler(IMapper mapper, IEntityCache entityCache)
    {
        _mapper = mapper;
        _entityCache = entityCache;
    }

    public async Task<Result<IReadOnlyList<TDto>>> Handle(TQuery request, CancellationToken cancellationToken)
    {
        var items = await _entityCache.GetAll<TDictionary>();
        return items.OrderBy(x => x.Key).Select(_mapper.Map<TDto>).ToList();
    }
}
