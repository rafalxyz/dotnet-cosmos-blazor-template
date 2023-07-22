using MyCompany.NewProject.Core.Model;

namespace MyCompany.NewProject.Core.Abstractions.Cache;

public static class EntityCacheExtensions
{
    public static async Task<TEntity?> SingleOrDefaultAsync<TEntity>(this IEntityCache cache, Func<TEntity, bool> predicate)
        where TEntity : Entity
    {
        return (await cache.GetAll<TEntity>()).SingleOrDefault(predicate);
    }
}