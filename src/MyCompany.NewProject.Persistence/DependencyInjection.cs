using MyCompany.NewProject.Persistence.Interceptors;
using MyCompany.NewProject.Persistence.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace MyCompany.NewProject.Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistence(this IServiceCollection services)
    {
        services.ConfigureOptions<DatabaseOptionsSetup>();
        services.AddOptions<DatabaseOptions>()
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services.AddScoped<EntitySaveChangesInterceptor>();
        services.AddDbContext<ApplicationDbContext>(
            (sp, options) =>
            {
                var databaseOptions = sp.GetRequiredService<IOptionsSnapshot<DatabaseOptions>>().Value;
                var interceptor = sp.GetRequiredService<EntitySaveChangesInterceptor>();
                options.UseCosmos(databaseOptions.ConnectionString, databaseOptions.DatabaseName)
                    .AddInterceptors(interceptor);
                options.EnableDetailedErrors(databaseOptions.EnableDetailedErrors);
                options.EnableSensitiveDataLogging(databaseOptions.EnableSensitiveDataLogging);
            });

        return services;
    }
}