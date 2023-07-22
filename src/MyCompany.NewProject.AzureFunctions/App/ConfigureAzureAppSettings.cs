using Azure.Identity;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace MyCompany.NewProject.AzureFunctions.App;

public static class ConfigureAzureAppSettings
{
    public static void AddAppAzureSettings(this IConfigurationBuilder builder, IHostEnvironment hostEnvironment)
    {
        if (hostEnvironment.IsDevelopment())
        {
            return;
        }
        var appConfigEndpoint = Environment.GetEnvironmentVariable(Shared.Constants.AppConfig.Endpoint);
        builder.AddAzureAppConfiguration(options =>
            options.Connect(new Uri(appConfigEndpoint!), new DefaultAzureCredential())
                .ConfigureRefresh(refresh =>
                    refresh.Register(Shared.Constants.AppConfig.TestAppSettingsSentinel, refreshAll: true))
                .ConfigureKeyVault(kv => kv.SetCredential(new DefaultAzureCredential())));
    }

    public static void AddAppAzureAppSettings(this IServiceCollection services, IHostEnvironment environment)
    {
        if (environment.IsDevelopment())
        {
            return;
        }
        services.AddAzureAppConfiguration();
    }

    public static void UseAppAzureAppSettings(this IFunctionsWorkerApplicationBuilder app, IHostEnvironment environment)
    {
        if (environment.IsDevelopment())
        {
            return;
        }
        app.UseAzureAppConfiguration();
    }
}