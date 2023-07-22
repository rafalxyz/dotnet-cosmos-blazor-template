using System.Text.Json;

namespace MyCompany.NewProject.WebApi.Tests.Extensions;

internal static class HttpClientExtensions
{
    public static async Task<T> GetJsonOrThrowAsync<T>(this HttpClient client, string endpoint)
    {
        var response = await client.GetAsync(endpoint);
        var content = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode)
        {
            throw new InvalidOperationException($"Request has failed. Response: {content}.");
        }
        var jsonSerializerOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        };
        return JsonSerializer.Deserialize<T>(content, jsonSerializerOptions)!;
    }
}
