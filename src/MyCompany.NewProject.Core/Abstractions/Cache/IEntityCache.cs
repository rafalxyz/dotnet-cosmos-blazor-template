using MyCompany.NewProject.Core.Model;

namespace MyCompany.NewProject.Core.Abstractions.Cache;

public sealed record EntityCacheKey(Type EntityType);

public interface IEntityCache
{
    Task<IReadOnlyList<TEntity>> GetAll<TEntity>() where TEntity : Entity;
}
