namespace MyCompany.NewProject.Core.Abstractions.Security;

public sealed record UserRole
{
    public static readonly UserRole ItAdmin = new("it-admin");
    public static readonly UserRole Admin = new("admin");
    public static readonly UserRole User = new("user");

    private static IReadOnlyList<UserRole> AllUserRoles => new List<UserRole> { ItAdmin, Admin, User };

    public static UserRole Parse(string value)
    {
        return AllUserRoles.SingleOrDefault(x => x.Value == value)
            ?? throw new InvalidOperationException($"Unknown app role: {value}.");
    }

    public string Value { get; }

    private UserRole(string value)
    {
        Value = value;
    }
}
