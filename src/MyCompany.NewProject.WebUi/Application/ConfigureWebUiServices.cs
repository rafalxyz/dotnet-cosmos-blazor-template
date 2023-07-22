using MyCompany.NewProject.Core.Abstractions.Security;
using MyCompany.NewProject.WebUi.Core.Messaging;
using MyCompany.NewProject.WebUi.Core.Notifications;
using MyCompany.NewProject.WebUi.Core.Security;
using MudBlazor.Services;

namespace MyCompany.NewProject.WebUi.Application;

public static class ConfigureWebUiServices
{
    public static void AddWebUi(this IServiceCollection services)
    {
        services.AddTransient<IDispatcher, Dispatcher>();
        services.AddScoped<ISnackbarService, SnackbarService>();
        services.AddScoped<IUserContextFactory, UserContextFactory>();
        services.AddMudServices(config =>
        {
            config.SnackbarConfiguration.ShowTransitionDuration = (int)TimeSpan.FromSeconds(0.5).TotalMilliseconds;
            config.SnackbarConfiguration.VisibleStateDuration = (int)TimeSpan.FromSeconds(2).TotalMilliseconds;
            config.SnackbarConfiguration.HideTransitionDuration = (int)TimeSpan.FromSeconds(0.5).TotalMilliseconds;
        });
    }
}
