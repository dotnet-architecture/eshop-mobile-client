using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using eShopOnContainers.Services;
using eShopOnContainers.Services.Settings;
using eShopOnContainers.ViewModels.Base;

namespace eShopOnContainers.ViewModels;

public class MainViewModel : ViewModelBase
{
    public ICommand SettingsCommand { get; }

    public MainViewModel(
        IDialogService dialogService, INavigationService navigationService, ISettingsService settingsService)
        : base(dialogService, navigationService, settingsService)
    {
        SettingsCommand = new AsyncRelayCommand(SettingsAsync);
    }

    private async Task SettingsAsync()
    {
        await NavigationService.NavigateToAsync("Settings");
    }
}