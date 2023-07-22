using Azure.Identity;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using MyCompany.NewProject.AzureFunctions.Shared.Options;

namespace MyCompany.NewProject.AzureFunctions.Shared.AdminAppApi;

internal sealed class NewProjectAppAuthorizationMessageHandler : DelegatingHandler
{
    private readonly NewProjectOptions _adminAppApiOptions;

    public NewProjectAppAuthorizationMessageHandler(
        IHostEnvironment hostEnvironment,
        IOptions<AzureAdOptions> azureAdOptions,
        IOptions<NewProjectOptions> adminAppApiOptions)
    {
        _adminAppApiOptions = adminAppApiOptions.Value;

        if (hostEnvironment.IsDevelopment())
        {
            Environment.SetEnvironmentVariable("AZURE_TENANT_ID", azureAdOptions.Value.TenantId);
            Environment.SetEnvironmentVariable("AZURE_CLIENT_ID", azureAdOptions.Value.ClientId);
            Environment.SetEnvironmentVariable("AZURE_CLIENT_SECRET", azureAdOptions.Value.ClientSecret);
        }
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var token = await RetrieveToken(cancellationToken);
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        return await base.SendAsync(request, cancellationToken);
    }

    public async Task<string> RetrieveToken(CancellationToken cancellationToken)
    {
        var credential = new DefaultAzureCredential();
        var accessToken = await credential.GetTokenAsync(new(new[] { _adminAppApiOptions.ApiScope }), cancellationToken);
        return accessToken.Token;
    }
}