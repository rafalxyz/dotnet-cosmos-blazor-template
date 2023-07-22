namespace MyCompany.NewProject.WebApi.Tests.Extensions;

internal static class HttpResponseMessageExtensions
{
    public static async Task ValidateAsync(this HttpResponseMessage response)
    {
        if (!response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            throw new InvalidOperationException($"Request has failed. Response: {content}.");
        }
    }
}
