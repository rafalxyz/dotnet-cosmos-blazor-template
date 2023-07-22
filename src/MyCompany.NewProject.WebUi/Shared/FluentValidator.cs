using Microsoft.AspNetCore.Components;
using FluentValidation;
using Microsoft.AspNetCore.Components.Forms;
using MyCompany.NewProject.Application.Shared.FluentValidation;

namespace MyCompany.NewProject.WebUi.Shared;

/// <summary>
/// Provides support for using fluent validation in Blazor forms.
/// </summary>
/// <remarks>
/// Based on <see href="https://blog.stevensanderson.com/2019/09/04/blazor-fluentvalidation/" />
/// </remarks>
public class FluentValidator<TModel> : ComponentBase
{
    [CascadingParameter] public required EditContext EditContext { get; init; }
    [Inject] public required IValidator<TModel> Validator { get; init; }

    protected override void OnInitialized()
    {
        var messageStore = new ValidationMessageStore(EditContext);

        EditContext.OnFieldChanged += (sender, eventArgs)
            => ValidateModelField((EditContext)sender!, messageStore, eventArgs.FieldIdentifier);

        EditContext.OnValidationRequested += (sender, eventArgs)
            => ValidateModel((EditContext)sender!, messageStore);
    }

    private void ValidateModelField(EditContext editContext, ValidationMessageStore messageStore, FieldIdentifier fieldIdentifier)
    {
        var fieldErrors = Validator.ValidateValue((TModel)editContext.Model!, fieldIdentifier.FieldName);
        messageStore.Clear(fieldIdentifier);
        foreach (var error in fieldErrors)
        {
            messageStore.Add(fieldIdentifier, error);
        }
        editContext.NotifyValidationStateChanged();
    }

    private void ValidateModel(EditContext editContext, ValidationMessageStore messageStore)
    {
        var modelErrors = Validator.Validate((TModel)editContext.Model);
        messageStore.Clear();
        foreach (var error in modelErrors.Errors)
        {
            var fieldIdentifier = editContext.Field(error.PropertyName);
            messageStore.Add(fieldIdentifier, error.ErrorMessage);
        }
        editContext.NotifyValidationStateChanged();
    }
}
