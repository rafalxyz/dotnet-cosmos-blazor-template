using System.Text.Json;
using System.Text.Json.Serialization;

namespace MyCompany.NewProject.WebUi.Application;

public static class ConfigureJsonOptions
{
    public static IMvcBuilder AddApplicationJsonOptions(this IMvcBuilder mvcBuilder)
    {
        return mvcBuilder.AddJsonOptions(opts =>
        {
            opts.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            opts.JsonSerializerOptions.Converters.Add(new TrimStringConverter());
        });
    }
}

public sealed class TrimStringConverter : JsonConverter<string?>
{
    public override string? Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options)
    {
        return reader.GetString()?.Trim();
    }

    public override void Write(
        Utf8JsonWriter writer,
        string? value,
        JsonSerializerOptions options)
    {
        writer.WriteStringValue(value);
    }
}