using MyCompany.NewProject.Core.Abstractions.Configuration;

namespace MyCompany.NewProject.AzureFunctions.Shared.Options;

internal sealed class AzureAdOptions : IAppConfiguration
{
    public static string SectionName => "AzureAd";

    public string? TenantId { get; init; }
    public string? ClientId { get; init; }
    public string? ClientSecret { get; init; }
}
