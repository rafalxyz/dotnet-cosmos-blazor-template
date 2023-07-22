using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace MyCompany.NewProject.AzureFunctions.Shared.NewProjectApi;

public class BaseNewProjectClient
{
    private readonly HttpClient _client;

    public BaseNewProjectClient(HttpClient client)
    {
        _client = client;
    }

    protected async Task Post<TModel>(string endpoint, TModel model)
    {
        var response = await _client.PostAsJsonAsync(endpoint, model);
        response.EnsureSuccessStatusCode();
    }
}
