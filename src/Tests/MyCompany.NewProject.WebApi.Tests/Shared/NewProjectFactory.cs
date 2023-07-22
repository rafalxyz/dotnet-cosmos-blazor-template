using MyCompany.NewProject.AzureFunctions.App;
using MyCompany.NewProject.AzureFunctions.Shared.Failures;
using MyCompany.NewProject.Core.Abstractions.DateTime;
using MyCompany.NewProject.WebApi.Tests.Services;
using MyCompany.NewProject.WebUi;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace MyCompany.NewProject.WebApi.Tests.Shared;

public class NewProjectFactory : WebApplicationFactory<IWebUiMarker>
{
    public const string TestEnvironmentName = "Tests";
    public const string TestAuthenticationSchemeName = "Tests";

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        var projectDirectory = Directory.GetCurrentDirectory();

        builder.UseEnvironment(TestEnvironmentName);

        builder.ConfigureAppConfiguration((_, configurationBuilder) =>
        {
            var configPath = Path.Combine(projectDirectory, "appsettings.json");
            configurationBuilder
                .AddJsonFile(configPath)
                .AddUserSecrets<NewProjectFactory>();
        });

        builder.ConfigureTestServices(services =>
        {
            services.RemoveAll<IDateTimeProvider>();
            services.AddTransient<IDateTimeProvider>(_ => new DateTimeProviderMock());

            // TODO: Service bus sample
            // services.RemoveAll<ServiceBusClient>();
            // services.AddTransient<ServiceBusClient>(_ => new ServiceBusClientMock());

            services.AddAuthentication(TestAuthenticationSchemeName)
                .AddScheme<AuthenticationSchemeOptions, TestAuthenticationHandler>(TestAuthenticationSchemeName, _ => { });

            ConfigureTestFunctionsServices(services, projectDirectory);
        });
    }

    private static void ConfigureTestFunctionsServices(IServiceCollection services, string projectDirectory)
    {
        var configPath = Path.Combine(projectDirectory, "functionsettings.json");
        var configuration = new ConfigurationBuilder()
            .AddJsonFile(configPath)
            .AddUserSecrets<NewProjectFactory>()
            .Build();

        services.AddFunctionsServices(configuration);

        services.RemoveAll<IFailureRepository>();
        services.AddTransient<IFailureRepository>(_ => new FailureRepositoryMock());
    }
}
