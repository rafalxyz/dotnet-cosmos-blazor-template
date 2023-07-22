using MyCompany.NewProject.Core.Model;

namespace MyCompany.NewProject.Core.Abstractions.Cache;

public interface IEntityCacheStore
{
    Task<IReadOnlyList<TEntity>> GetOrCreateAsync<TEntity>(Func<Task<IReadOnlyList<TEntity>>> create) where TEntity : Entity;
    void Invalidate(EntityCacheKey cacheKey);
}
