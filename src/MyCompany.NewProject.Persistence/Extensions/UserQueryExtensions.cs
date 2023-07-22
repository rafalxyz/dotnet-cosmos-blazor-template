using MyCompany.NewProject.Core.Model.Users;
using Microsoft.EntityFrameworkCore;

namespace MyCompany.NewProject.Persistence.Extensions;

public static class UserQueryExtensions
{
    public static async Task<User?> GetByEmailAsync(this IQueryable<User> query, string email, CancellationToken cancellationToken = default)
    {
        var emailNormalized = email.Trim().ToLowerInvariant();
        return await query.SingleOrDefaultAsync(x => x.Email == emailNormalized, cancellationToken);
    }
}
