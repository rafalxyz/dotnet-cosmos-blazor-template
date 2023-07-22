using MyCompany.NewProject.Application.Features.Users;
using MyCompany.NewProject.Core.Results;
using MyCompany.NewProject.WebUi.Core.Messaging;
using MyCompany.NewProject.WebUi.Core.Notifications;
using MyCompany.NewProject.WebUi.Shared.Dialogs;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace MyCompany.NewProject.WebUi.Features.Users;

public partial class UsersPage
{
    [Inject] public required IDispatcher Dispatcher { get; init; }
    [Inject] public required IDialogService DialogService { get; init; }
    [Inject] public required ISnackbarService SnackbarService { get; init; }

    private IReadOnlyList<UserDto>? _users;

    protected override async Task OnInitializedAsync()
    {
        await LoadUsers();
    }

    private async Task LoadUsers()
    {
        _users = await Dispatcher.Send(new GetUsersQuery()).ValueOrDefault(() => new List<UserDto>());
    }

    private async Task OpenUserFormDialog(UserDto? user)
    {
        var isEdit = user is not null;

        var parameters = new DialogParameters
        {
            [nameof(UserFormDialog.User)] = user
        };

        var dialog = await DialogService.ShowAsync<UserFormDialog>(isEdit ? "Edit User" : "Add User", parameters);
        var result = await dialog.Result;

        if (result.Canceled)
        {
            return;
        }

        SnackbarService.ShowSuccess(isEdit ? "User edited successfully." : "User added successfully.");
        await LoadUsers();
    }

    private async Task OpenDeleteUserDialog(UserDto user)
    {
        var onConfirm = () => Dispatcher.Send(new DeleteUserCommand { UserId = user.Id });

        var parameters = new DialogParameters
        {
            [nameof(ConfirmationDialog.Title)] = "Delete user",
            [nameof(ConfirmationDialog.ConfirmationText)] = $"Are you sure you want to delete user {user.DisplayName}?",
            [nameof(ConfirmationDialog.ButtonText)] = "Yes, delete",
            [nameof(ConfirmationDialog.OnConfirm)] = onConfirm
        };

        var dialog = await DialogService.ShowAsync<ConfirmationDialog>("Delete User", parameters);
        var result = await dialog.Result;

        if (result.Canceled)
        {
            return;
        }

        SnackbarService.ShowSuccess("User deleted successfully.");
        await LoadUsers();
    }
}