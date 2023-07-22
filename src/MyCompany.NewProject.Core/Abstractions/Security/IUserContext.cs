namespace MyCompany.NewProject.Core.Abstractions.Security;

public interface IUserContext
{
    string Id { get; }
    string Email { get; }
    string Name { get; }
    IReadOnlyList<UserRole> Roles { get; }
}
