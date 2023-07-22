using MyCompany.NewProject.Core.Abstractions.Cache;
using MyCompany.NewProject.Core.Model;
using MyCompany.NewProject.Persistence;
using Microsoft.EntityFrameworkCore;

namespace MyCompany.NewProject.Infrastructure.Cache;

internal sealed class MemoryEntityCache : IEntityCache
{
    private readonly IEntityCacheStore _entityCacheStore;
    private readonly ApplicationDbContext _db;

    public MemoryEntityCache(IEntityCacheStore entityCacheStore, ApplicationDbContext db)
    {
        _entityCacheStore = entityCacheStore;
        _db = db;
    }

    public async Task<IReadOnlyList<TEntity>> GetAll<TEntity>()
        where TEntity : Entity
    {
        return await _entityCacheStore.GetOrCreateAsync<TEntity>(async () => (await _db.Set<TEntity>().ToListAsync()).AsReadOnly());
    }
}
