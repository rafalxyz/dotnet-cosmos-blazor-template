using MyCompany.NewProject.Core.Abstractions.Cache;
using MyCompany.NewProject.Core.Model;
using Microsoft.Extensions.Caching.Memory;

namespace MyCompany.NewProject.Infrastructure.Cache;

internal sealed class MemoryEntityCacheStore : IEntityCacheStore
{
    private readonly IMemoryCache _memoryCache;

    public MemoryEntityCacheStore(IMemoryCache memoryCache)
    {
        _memoryCache = memoryCache;
    }

    public async Task<IReadOnlyList<TEntity>> GetOrCreateAsync<TEntity>(Func<Task<IReadOnlyList<TEntity>>> create) where TEntity : Entity
    {
        return (await _memoryCache.GetOrCreateAsync(new EntityCacheKey(typeof(TEntity)), async _ => await create()))!;
    }

    public void Invalidate(EntityCacheKey cacheKey)
    {
        _memoryCache.Remove(cacheKey);
    }
}