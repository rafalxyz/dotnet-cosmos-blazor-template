using MyCompany.NewProject.Application.Abstractions.Messaging;
using MyCompany.NewProject.Application.Shared.FluentValidation;
using MyCompany.NewProject.WebUi.Core.Messaging;
using FluentValidation;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace MyCompany.NewProject.WebUi.Shared.Dialogs;

public class FormDialogBase<TCommand, TResponse> : ComponentBase
    where TCommand : ICommand<TResponse>
{
    [Inject] public required IDispatcher Dispatcher { get; init; }
    [Inject] public required IValidator<TCommand> Validator { get; init; }

    protected TCommand Command { get; set; } = default!;
    protected EditContext EditContext { get; set; } = default!;
    protected ValidationMessageStore ValidationMessageStore { get; set; } = default!;

    protected void InitializeForm(TCommand command)
    {
        Command = command;
        EditContext = new EditContext(command);
        ValidationMessageStore = new ValidationMessageStore(EditContext);
    }

    protected bool IsPropertyValid(string propertyName)
    {
        var validationErrors = Validator.ValidateValue(Command, propertyName);
        return !validationErrors.Any();
    }

    protected void SetPropertyAsyncError(string propertyName, string errorMessage)
    {
        ValidationMessageStore.Clear(EditContext.Field(propertyName));
        ValidationMessageStore.Add(EditContext.Field(propertyName), errorMessage);
    }

    protected void ClearPropertyAsyncError(string propertyName)
    {
        ValidationMessageStore.Clear(EditContext.Field(propertyName));
    }
}
