using MyCompany.NewProject.Core.Abstractions.Configuration;
using System.ComponentModel.DataAnnotations;

namespace MyCompany.NewProject.AzureFunctions.Shared.Options;

internal sealed class NewProjectOptions : IAppConfiguration
{
    public static string SectionName => "NewProject";

    [Required]
    public required string BaseApiUrl { get; init; }

    [Required]
    public required string ApiScope { get; init; }
}