using AutoMapper;
using FluentValidation;
using MyCompany.NewProject.Application.Shared.FluentValidation.Validators;
using MyCompany.NewProject.Core.Model.Dictionaries;

namespace MyCompany.NewProject.Application.Features.Dictionaries.Locations;

public sealed class UpsertLocationCommand : DictionaryUpsertCommandBase
{
    public required string Name { get; set; }
    public required string CountryId { get; set; }
}

public sealed class UpsertLocationCommandValidator : AbstractValidator<UpsertLocationCommand>
{
    public UpsertLocationCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().NoWhiteSpaceAtBeginningOrEnd();
        RuleFor(x => x.CountryId).NotEmpty();
    }
}

internal sealed class UpsertLocationCommandHandler : DictionaryUpsertCommandHandler<UpsertLocationCommand, Location>
{
    public UpsertLocationCommandHandler(IMapper mapper, IDictionaryService dictionaryService)
        : base(mapper, dictionaryService) { }
}
