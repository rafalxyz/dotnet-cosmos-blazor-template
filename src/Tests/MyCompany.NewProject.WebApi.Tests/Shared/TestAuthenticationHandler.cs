using MyCompany.NewProject.Core.Abstractions.Security;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace MyCompany.NewProject.WebApi.Tests.Shared;

internal class TestAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    public TestAuthenticationHandler(IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock)
        : base(options, logger, encoder, clock)
    {
    }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Role, "access_as_application"),
            new Claim(ClaimTypes.Role, UserRole.ItAdmin.Value),
        };

        var identity = new ClaimsIdentity(claims, NewProjectFactory.TestAuthenticationSchemeName);
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, NewProjectFactory.TestAuthenticationSchemeName);

        var result = AuthenticateResult.Success(ticket);
        return Task.FromResult(result);
    }
}
