using MyCompany.NewProject.AzureFunctions.Shared.AdminAppApi;
using MyCompany.NewProject.AzureFunctions.Shared.ChangeFeed;
using MyCompany.NewProject.AzureFunctions.Shared.Failures;
using MyCompany.NewProject.AzureFunctions.Shared.Options;
using MyCompany.NewProject.AzureFunctions.Shared.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Polly;
using Polly.Extensions.Http;
using System;

namespace MyCompany.NewProject.AzureFunctions.App;

public static class ConfigureFunctionsServices
{
    private const int RetryCount = 6;

    public static IServiceCollection AddFunctionsServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<Program>());
        services.AddTransient<IEntityChangeFeedProcessor, EntityChangeFeedProcessor>();
        services.AddTransient<IFailureRepository, FailureRepository>();

        services.Configure<AzureAdOptions>(configuration.GetSection(AzureAdOptions.SectionName));

        services.AddOptions<NewProjectOptions>()
            .Bind(configuration.GetSection(NewProjectOptions.SectionName))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services.ConfigureOptions<DatabaseOptionsSetup>();
        services.AddOptions<DatabaseOptions>()
            .ValidateDataAnnotations()
            .ValidateOnStart();
        services.AddDbContext<FunctionsDbContext>(
            (sp, options) =>
            {
                var databaseOptions = sp.GetRequiredService<IOptionsSnapshot<DatabaseOptions>>().Value;
                options.UseCosmos(
                    databaseOptions.ConnectionString,
                    databaseOptions.DatabaseName);
            });

        services.AddSingleton<NewProjectAppAuthorizationMessageHandler>();

        return services;
    }

    private static IServiceCollection AddAppClient<TClient, TImplementation>(this IServiceCollection services, IConfiguration configuration)
        where TClient : class
        where TImplementation : class, TClient
    {
        var newProjectOptions = configuration.GetRequiredSection(NewProjectOptions.SectionName)
            .Get<NewProjectOptions>()!;

        var retryPolicy = HttpPolicyExtensions
            .HandleTransientHttpError()
            .WaitAndRetryAsync(RetryCount, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));

        services
            .AddHttpClient<TClient, TImplementation>()
            .ConfigureHttpClient(c => c.BaseAddress = new Uri(newProjectOptions.BaseApiUrl))
            .AddPolicyHandler(retryPolicy)
            .AddHttpMessageHandler<NewProjectAppAuthorizationMessageHandler>();

        return services;
    }
}