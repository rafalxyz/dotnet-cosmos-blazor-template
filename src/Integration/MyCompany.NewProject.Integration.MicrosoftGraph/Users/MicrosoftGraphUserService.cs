using Microsoft.Graph;
using Microsoft.Graph.Models;
using MyCompany.NewProject.Core.Results;
using MyCompany.NewProject.Core.Results.Errors;

namespace MyCompany.NewProject.Integration.MicrosoftGraph.Users;

public interface IMicrosoftGraphUserService
{
    Task<Result<UserDto>> GetByEmailAsync(string email);
}

internal sealed class MicrosoftGraphUserService : IMicrosoftGraphUserService
{
    private readonly GraphServiceClient _graphServiceClient;

    public MicrosoftGraphUserService(GraphServiceClient graphServiceClient)
    {
        _graphServiceClient = graphServiceClient;
    }

    public async Task<Result<UserDto>> GetByEmailAsync(string email)
    {
        UserCollectionResponse? response = await _graphServiceClient.Users.GetAsync(config
            => config.QueryParameters.Filter = $"mail eq '{email}'");

        if (response?.Value?.Any() != true)
        {
            return new NotFoundError("User with the given email does not exist in MS Graph.");
        }

        if (response.Value.Count > 1)
        {
            return new ValidationError("There is more than one user with the given email in MS Graph.");
        }

        var user = response.Value.Single();
        return new UserDto(Id: user.Id!, DisplayName: user.DisplayName!, Email: user.Mail!);
    }
}
