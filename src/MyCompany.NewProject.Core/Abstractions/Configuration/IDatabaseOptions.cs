using System.ComponentModel.DataAnnotations;

namespace MyCompany.NewProject.Core.Abstractions.Configuration;

public interface IDatabaseOptions : IAppConfiguration
{
    [Required]
    string ConnectionString { get; set; }
    [Required]
    string DatabaseName { get; set; }
}