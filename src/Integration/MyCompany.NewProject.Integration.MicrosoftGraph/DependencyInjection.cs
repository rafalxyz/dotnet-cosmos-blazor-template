using Azure.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Graph;
using MyCompany.NewProject.Integration.MicrosoftGraph.Options;
using MyCompany.NewProject.Integration.MicrosoftGraph.Users;

namespace MyCompany.NewProject.Integration.MicrosoftGraph;

public static class DependencyInjection
{
    public static IServiceCollection AddMicrosoftGraphIntegration(
        this IServiceCollection services,
        IConfiguration configuration,
        IHostEnvironment hostEnvironment)
    {
        var options = configuration.GetRequiredSection(MicrosoftGraphOptions.SectionName).Get<MicrosoftGraphOptions>()!;

        // TODO: Setup managed identity and use client secret credentials only in local environment
        if (hostEnvironment.IsDevelopment() || hostEnvironment.IsProduction())
        {
            Environment.SetEnvironmentVariable("AZURE_TENANT_ID", options.TenantId);
            Environment.SetEnvironmentVariable("AZURE_CLIENT_ID", options.ClientId);
            Environment.SetEnvironmentVariable("AZURE_CLIENT_SECRET", options.ClientSecret);
        }

        services.AddSingleton(_ => new GraphServiceClient(new DefaultAzureCredential()));
        services.AddSingleton<IMicrosoftGraphUserService, MicrosoftGraphUserService>();

        return services;
    }
}