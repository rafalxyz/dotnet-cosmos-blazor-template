using MyCompany.NewProject.WebApi.Tests.Extensions;
using MyCompany.NewProject.WebApi.Tests.Shared;
using Newtonsoft.Json.Linq;

namespace MyCompany.NewProject.WebApi.Tests.AwarenessTests;

public sealed class OpenApiAwarenessTests : BaseApiTests
{
    private readonly HttpClient _client;
    private const string SwaggerFilename = "swagger.json";
    private const string TestDirectory = "AwarenessTests";

    public OpenApiAwarenessTests(NewProjectFactory factory)
        : base(factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetOpenApiJson()
    {
        var response = await _client.GetAsync("swagger/v1/swagger.json");
        await response.ValidateAsync();
        var newSwagger = await response.Content.ReadAsStringAsync();
        newSwagger.Should().NotBeNullOrEmpty();

        var swaggerPath = Path.Combine(GetProjectPath(), TestDirectory, SwaggerFilename);
        File.Exists(swaggerPath).Should().BeTrue($"Swagger file at {swaggerPath} does not exist. Please ensure the file is correctly generated before running the test.");

        var oldJToken = JToken.Parse(File.ReadAllText(swaggerPath));
        var newJToken = JToken.Parse(newSwagger);

        JToken.DeepEquals(oldJToken, newJToken).Should().BeTrue($"The API has changed. Please verify if the changes were expected. If so, regenerate {SwaggerFilename} file.");
    }

    private static string GetProjectPath()
    {
        var assembly = System.Reflection.Assembly.GetExecutingAssembly();
        var assemblyPath = new FileInfo(assembly.Location).Directory!.FullName;
        return Directory.GetParent(assemblyPath)!.Parent!.Parent!.FullName;
    }
}