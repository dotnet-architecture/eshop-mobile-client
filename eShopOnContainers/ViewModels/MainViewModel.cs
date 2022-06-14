using eShopOnContainers.Models.Navigation;
using eShopOnContainers.ViewModels.Base;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Maui;
using eShopOnContainers.Services;
using eShopOnContainers.Services.Settings;
using CommunityToolkit.Mvvm.Input;

namespace eShopOnContainers.ViewModels
{
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
}