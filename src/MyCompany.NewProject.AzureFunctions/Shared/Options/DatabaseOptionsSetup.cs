using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;

namespace MyCompany.NewProject.AzureFunctions.Shared.Options;

internal sealed class DatabaseOptionsSetup : IConfigureOptions<DatabaseOptions>
{
    private readonly IConfiguration _configuration;

    public DatabaseOptionsSetup(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void Configure(DatabaseOptions options)
    {
        var connectionString = _configuration.GetConnectionString("Database");
        ArgumentException.ThrowIfNullOrEmpty(connectionString);

        options.ConnectionString = connectionString;
        _configuration.GetRequiredSection(DatabaseOptions.SectionName).Bind(options);
    }
}
