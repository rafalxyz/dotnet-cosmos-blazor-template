using MyCompany.NewProject.Core.Abstractions.Security;
using Microsoft.AspNetCore.Components.Authorization;

namespace MyCompany.NewProject.WebUi.Core.Security;

internal sealed class UserContextFactory : IUserContextFactory
{
    private readonly AuthenticationStateProvider _authenticationStateProvider;

    public UserContextFactory(AuthenticationStateProvider authenticationStateProvider)
    {
        _authenticationStateProvider = authenticationStateProvider;
    }

    public async Task<IUserContext> Create()
    {
        var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
        return new UserContext(authState.User);
    }
}