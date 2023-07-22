using MyCompany.NewProject.Core.Model.Users;

namespace MyCompany.NewProject.DatabaseSeeder.Seeders;

public static class UsersSeeder
{
    public const string TestUserOneId = "4db593ac-298e-42b7-90ad-3bdbad3447ef";
    public const string TestUserOneAzureActiveDirectoryId = "17c8b087-bcdd-42ad-8dfc-01878a2360c7";

    public const string TestUserTwoId = "d9bb88a5-fc22-44cb-9dfc-5bcd2d9fe294";
    public const string TestUserTwoAzureActiveDirectoryId = "238ead42-c7b1-4db3-b497-7cc2ecc721e8";

    public static IReadOnlyList<User> GetUsers()
    {
        var testUserOne = User.Create("User One", TestUserOneAzureActiveDirectoryId, "user.one@mycompany.com");
        testUserOne.Id = TestUserOneId;

        var testUserTwo = User.Create("User Two", TestUserTwoAzureActiveDirectoryId, "user.two@mycompany.com");
        testUserTwo.Id = TestUserTwoId;

        return new List<User> { testUserOne, testUserTwo };
    }
}