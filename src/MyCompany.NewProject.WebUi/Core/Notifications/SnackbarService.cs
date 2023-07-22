using MudBlazor;

namespace MyCompany.NewProject.WebUi.Core.Notifications;

public interface ISnackbarService
{
    void ShowSuccess(string message);
    void ShowError(string message);
    void ShowInfo(string message);
    void ShowWarning(string message);
}

internal sealed class SnackbarService : ISnackbarService
{
    private readonly ISnackbar _snackbar;

    public SnackbarService(ISnackbar snackbar)
    {
        _snackbar = snackbar;
    }

    public void ShowSuccess(string message)
    {
        _snackbar.Add(message, Severity.Success);
    }

    public void ShowError(string message)
    {
        _snackbar.Add(message, Severity.Error);
    }

    public void ShowInfo(string message)
    {
        _snackbar.Add(message, Severity.Info);
    }

    public void ShowWarning(string message)
    {
        _snackbar.Add(message, Severity.Warning);
    }
}
