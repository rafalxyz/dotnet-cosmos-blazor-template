using MyCompany.NewProject.Core.Abstractions.Security;
using System.Security.Claims;

namespace MyCompany.NewProject.WebUi.Core.Security;

internal sealed class UserContext : IUserContext
{
    private const string ObjectIdentifierClaimType = "http://schemas.microsoft.com/identity/claims/objectidentifier";

    public string Id { get; }
    public string Email { get; }
    public string Name { get; }
    public IReadOnlyList<UserRole> Roles { get; }

    public UserContext(ClaimsPrincipal user)
    {
        Id = user.FindFirst(ObjectIdentifierClaimType)!.Value;
        Email = user.FindFirst("name")!.Value;
        Name = user.FindFirst("preferred_username")!.Value;
        Roles = user.FindAll(ClaimTypes.Role).Select(claim => UserRole.Parse(claim.Value)).ToList().AsReadOnly();
    }
}
