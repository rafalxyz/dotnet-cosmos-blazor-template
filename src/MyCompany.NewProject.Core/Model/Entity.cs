using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MyCompany.NewProject.Core.Model;

public abstract class Entity
{
    public string Id { get; set; } = null!;
    [System.Text.Json.Serialization.JsonPropertyName("_etag"), Newtonsoft.Json.JsonProperty("_etag")]
    public string EntityTag { get; set; } = null!;
    public DateTimeOffset LastModified { get; set; } = DateTimeOffset.UtcNow;

    [NotMapped]
    public byte[] Version
    {
        get => Encoding.UTF8.GetBytes(EntityTag ?? string.Empty);
        set => EntityTag = Encoding.UTF8.GetString(value);
    }
}