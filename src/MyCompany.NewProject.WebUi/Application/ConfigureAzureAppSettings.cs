using Azure.Identity;

namespace MyCompany.NewProject.WebUi.Application;

public static class ConfigureAzureAppSettings
{
    public static void AddAppAzureSettings(this WebApplicationBuilder webApplicationBuilder)
    {
        if (webApplicationBuilder.Environment.IsDevelopment())
        {
            return;
        }

        var appConfigEndpoint = webApplicationBuilder.Configuration["AppConfig:Endpoint"];
        webApplicationBuilder.Configuration.AddAzureAppConfiguration(options =>
        {
            options.Connect(new Uri(appConfigEndpoint!), new DefaultAzureCredential())
                .ConfigureRefresh(refresh =>
                    refresh
                        .Register("TestApp:Settings:Sentinel", refreshAll: true)
                        .SetCacheExpiration(TimeSpan.FromDays(1)))
                .ConfigureKeyVault(kv => kv.SetCredential(new DefaultAzureCredential()));
        });
    }

    public static void AddAppAzureAppSettings(this IServiceCollection services, IWebHostEnvironment environment)
    {
        if (environment.IsDevelopment())
        {
            return;
        }
        services.AddAzureAppConfiguration();
    }

    public static void UseAppAzureAppSettings(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            return;
        }
        app.UseAzureAppConfiguration();
    }
}
