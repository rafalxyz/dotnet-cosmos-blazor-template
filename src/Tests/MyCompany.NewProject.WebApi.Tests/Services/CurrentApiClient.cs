namespace MyCompany.NewProject.WebApi.Tests.Services;

internal static class CurrentApiClient
{
    private static HttpClient? _value;
    public static HttpClient Value => _value
        ?? throw new InvalidOperationException("Current http client is not set.");

    public static void Set(HttpClient value)
    {
        _value = value;
    }
}
