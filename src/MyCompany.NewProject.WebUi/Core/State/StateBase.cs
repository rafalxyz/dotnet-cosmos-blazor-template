namespace MyCompany.NewProject.WebUi.Core.State;

public abstract class StateBase
{
    public event Action? OnChange;
    protected void NotifyStateChanged() => OnChange?.Invoke();
}
