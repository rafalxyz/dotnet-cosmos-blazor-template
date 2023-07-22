using MyCompany.NewProject.Core.Model;
using MyCompany.NewProject.Persistence;
using Microsoft.EntityFrameworkCore;

namespace MyCompany.NewProject.WebApi.Tests.Extensions;

internal static class ApplicationDbContextExtensions
{
    public static async Task RemoveAllAsync<TEntity>(this ApplicationDbContext context)
        where TEntity : Entity
    {
        var entities = await context.Set<TEntity>().ToListAsync();
        context.RemoveRange(entities);
        await context.SaveChangesAsync();
    }

    public static async Task RemoveByIdAsync<TEntity>(this ApplicationDbContext context, string id)
        where TEntity : Entity
    {
        var entity = await context.Set<TEntity>().SingleOrDefaultAsync(x => x.Id == id);
        if (entity is null)
        {
            return;
        }
        context.Remove(entity);
        await context.SaveChangesAsync();
    }
}
