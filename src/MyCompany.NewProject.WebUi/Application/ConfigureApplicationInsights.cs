using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Extensibility;

namespace MyCompany.NewProject.WebUi.Application;

public static class ConfigureApplicationInsights
{
    public static void AddApplicationInsights(this IServiceCollection services)
    {
        services.AddApplicationInsightsTelemetry();
    }

    public static void LogToApplicationInsightsOnError(this WebApplicationBuilder builder, Exception exception)
    {
        var applicationInsightsConnectionString = builder.Configuration["ApplicationInsights:ConnectionString"];
        if (string.IsNullOrWhiteSpace(applicationInsightsConnectionString))
        {
            return;
        }
        var telemetryConfiguration = TelemetryConfiguration.CreateDefault();
        telemetryConfiguration.ConnectionString = applicationInsightsConnectionString;
        var telemetryClient = new TelemetryClient(telemetryConfiguration);
        telemetryClient.TrackException(exception);
        telemetryClient.Flush();
    }
}