namespace MyCompany.NewProject.Core.Abstractions.Security;

public interface IUserContextFactory
{
    Task<IUserContext> Create();
}
