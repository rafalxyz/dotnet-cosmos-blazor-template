using MudBlazor;

namespace MyCompany.NewProject.WebUi.Core.MudBlazor;

public static class FormIcons
{
    public static string AddOrEdit(object? resource)
    {
        return resource is null ? Icons.Material.Filled.Add : Icons.Material.Filled.Edit;
    }

    public static string Edit => Icons.Material.Filled.Edit;
}
