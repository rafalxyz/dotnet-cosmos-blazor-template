using MyCompany.NewProject.Core.Abstractions.Configuration;

namespace MyCompany.NewProject.Integration.MicrosoftGraph.Options;

internal sealed class MicrosoftGraphOptions : IAppConfiguration
{
    public static string SectionName => "MicrosoftGraph";

    public required string ClientId { get; init; }
    public required string TenantId { get; init; }
    public string? ClientSecret { get; init; }
}