using MyCompany.NewProject.WebApi.Tests.Services;
using Microsoft.AspNetCore.Mvc.Testing;

namespace MyCompany.NewProject.WebApi.Tests.Extensions;

internal static class WebApplicationFactoryExtensions
{
    public static HttpClient CreateApiClient<T>(this WebApplicationFactory<T> factory)
        where T : class
    {
        var client = factory.CreateClient();
        client.BaseAddress = new Uri(client.BaseAddress!, "api/");
        CurrentApiClient.Set(client);
        return client;
    }
}
