using MyCompany.NewProject.Core.Abstractions.Security;
using MyCompany.NewProject.WebApi.Security;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Identity.Web;

namespace MyCompany.NewProject.WebUi.Application;

public static class ConfigureAuthentication
{
    public static void AddAppAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        var authBuilder = services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme);

        authBuilder.AddMicrosoftIdentityWebApp(options =>
        {
            configuration.GetSection("AzureAd").Bind(options);
            var defaultBackChannel = new HttpClient();
            defaultBackChannel.DefaultRequestHeaders.Add("Origin", options.Domain);
            options.Backchannel = defaultBackChannel;
        });

        authBuilder.AddMicrosoftIdentityWebApi(configuration);

        services.AddAuthorization(policies =>
        {
            policies.FallbackPolicy = new AuthorizationPolicyBuilder()
                .RequireRole(UserRole.ItAdmin.Value)
                .Build();

            policies.AddPolicy(Policies.ApiAccess, p =>
            {
                p.RequireRole("access_as_application");
            });
        });
    }
}
