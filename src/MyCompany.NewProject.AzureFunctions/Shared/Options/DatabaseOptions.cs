using MyCompany.NewProject.Core.Abstractions.Configuration;

namespace MyCompany.NewProject.AzureFunctions.Shared.Options;

internal sealed class DatabaseOptions : IDatabaseOptions
{
    public static string SectionName => "Database";

    public required string ConnectionString { get; set; }
    public required string DatabaseName { get; set; }
}