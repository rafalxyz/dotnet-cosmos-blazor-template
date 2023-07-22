using FluentValidation;
using FluentValidation.Validators;

namespace MyCompany.NewProject.Application.Shared.FluentValidation.Validators;

public sealed class NoWhiteSpaceAtBeginningOrEndValidator<T> : PropertyValidator<T, string>
{
    public override string Name => "NoWhiteSpaceAtBeginningOrEnd";

    public override bool IsValid(ValidationContext<T> context, string value)
    {
        return value.Trim() == value;
    }

    protected override string GetDefaultMessageTemplate(string errorCode)
    {
        return "Value must not contain white spaces at the beginning or end.";
    }
}

public static class ValidatorExtensions
{
    public static IRuleBuilderOptions<T, string> NoWhiteSpaceAtBeginningOrEnd<T>(this IRuleBuilder<T, string> ruleBuilder)
        => ruleBuilder.SetValidator(new NoWhiteSpaceAtBeginningOrEndValidator<T>());
}
