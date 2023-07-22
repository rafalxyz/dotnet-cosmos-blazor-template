namespace MyCompany.NewProject.Core.Model.Users;

public sealed class User : Entity, IDeletable
{
    public string DisplayName { get; private set; }
    public string AzureActiveDirectoryUserId { get; private set; }
    public string Email { get; private set; }
    public bool Deleted { get; private set; }

#nullable disable

    // For EF Core
    private User()
    { }

#nullable enable

    public static User Create(
        string displayName,
        string azureActiveDirectoryUserId,
        string email)
    {
        return new User
        {
            Id = Guid.NewGuid().ToString(),
            DisplayName = displayName,
            AzureActiveDirectoryUserId = azureActiveDirectoryUserId,
            Email = email,
        };
    }

    public void Update(
        string displayName,
        string azureActiveDirectoryUserId,
        string email)
    {
        DisplayName = displayName;
        AzureActiveDirectoryUserId = azureActiveDirectoryUserId;
        Email = email;
    }

    public void Delete()
    {
        Deleted = true;
    }
}