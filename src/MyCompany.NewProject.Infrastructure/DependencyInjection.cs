using MyCompany.NewProject.Core.Abstractions.Cache;
using MyCompany.NewProject.Core.Abstractions.DateTime;
using MyCompany.NewProject.Infrastructure.Cache;
using MyCompany.NewProject.Infrastructure.DateTime;
using Microsoft.Extensions.DependencyInjection;

namespace MyCompany.NewProject.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
        services.AddSingleton<IEntityCacheStore, MemoryEntityCacheStore>();
        services.AddScoped<IEntityCache, MemoryEntityCache>();

        return services;
    }
}
