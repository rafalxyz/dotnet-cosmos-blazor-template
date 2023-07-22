using MyCompany.NewProject.Core.Abstractions.Configuration;

namespace MyCompany.NewProject.Persistence.Options;

public sealed class DatabaseOptions : IDatabaseOptions
{
    public static string SectionName => "Database";

    public required string ConnectionString { get; set; }
    public bool EnableDetailedErrors { get; set; }
    public bool EnableSensitiveDataLogging { get; set; }
    public required string DatabaseName { get; set; }
}
