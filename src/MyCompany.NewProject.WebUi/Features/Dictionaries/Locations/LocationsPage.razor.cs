using Microsoft.AspNetCore.Components;
using MudBlazor;
using MyCompany.NewProject.Application.Features.Dictionaries.Locations;
using MyCompany.NewProject.Core.Results;
using MyCompany.NewProject.WebUi.Core.Messaging;
using MyCompany.NewProject.WebUi.Core.Notifications;
using MyCompany.NewProject.WebUi.Shared.Dialogs;

namespace MyCompany.NewProject.WebUi.Features.Dictionaries.Locations;

public partial class LocationsPage
{
    [Inject] public required IDispatcher Dispatcher { get; init; }
    [Inject] public required IDialogService DialogService { get; init; }
    [Inject] public required ISnackbarService SnackbarService { get; init; }

    private IReadOnlyList<LocationDto>? _locations;

    protected override async Task OnInitializedAsync()
    {
        await LoadLocations();
    }

    private async Task LoadLocations()
    {
        _locations = await Dispatcher.Send(new GetAllLocationsQuery()).ValueOrDefault(() => new List<LocationDto>());
    }

    private async Task OpenLocationFormDialog(LocationDto location)
    {
        var parameters = new DialogParameters
        {
            [nameof(LocationFormDialog.Location)] = location
        };

        var dialog = await DialogService.ShowAsync<LocationFormDialog>("Edit Location", parameters);
        var result = await dialog.Result;
        if (result.Canceled)
        {
            return;
        }

        SnackbarService.ShowSuccess("Location edited successfully.");
        await LoadLocations();
    }

    private async Task OpenLocationDeleteDialog(LocationDto location)
    {
        var onConfirm = () => Dispatcher.Send(new DeleteLocationCommand(location.Id));

        var parameters = new DialogParameters
        {
            [nameof(ConfirmationDialog.Title)] = "Delete location",
            [nameof(ConfirmationDialog.ConfirmationText)] = $"Are you sure you want to delete location {location.Name}?",
            [nameof(ConfirmationDialog.ButtonText)] = "Yes, delete",
            [nameof(ConfirmationDialog.OnConfirm)] = onConfirm
        };

        var dialog = await DialogService.ShowAsync<ConfirmationDialog>("Delete Location", parameters);
        var result = await dialog.Result;

        if (result.Canceled)
        {
            return;
        }

        SnackbarService.ShowSuccess("Location deleted successfully.");
        await LoadLocations();
    }
}