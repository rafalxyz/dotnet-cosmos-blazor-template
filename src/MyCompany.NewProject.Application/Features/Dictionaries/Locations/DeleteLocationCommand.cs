using MyCompany.NewProject.Core.Model.Dictionaries;
using MyCompany.NewProject.Persistence;

namespace MyCompany.NewProject.Application.Features.Dictionaries.Locations;

public sealed record DeleteLocationCommand(string Id) : IDictionaryDeleteCommand;

internal sealed class DeleteLocationCommandHandler : DictionaryDeleteCommandHandler<DeleteLocationCommand, Location>
{
    public DeleteLocationCommandHandler(ApplicationDbContext db) : base(db) { }
}
