using FluentValidation;

namespace MyCompany.NewProject.Application.Shared.FluentValidation;

/// <remarks>
/// Required to make FluentValidation work with Blazor forms.
/// </remarks>
public static class ValidatorExtensions
{
    public static IEnumerable<string> ValidateValue<T>(this IValidator<T> validator, object model, string propertyName)
    {
        var result = validator.Validate(ValidationContext<T>.CreateWithOptions((T)model, x => x.IncludeProperties(propertyName)));
        return result.IsValid
            ? (IEnumerable<string>)Array.Empty<string>()
            : result.Errors.Select(e => e.ErrorMessage);
    }
}
