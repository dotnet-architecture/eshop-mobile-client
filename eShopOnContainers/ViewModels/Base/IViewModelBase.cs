using System;
using eShopOnContainers.Services;
using eShopOnContainers.Services.Settings;

namespace eShopOnContainers.ViewModels.Base
{
    public interface IViewModelBase : IQueryAttributable
    {
        public IDialogService DialogService { get; }
        public INavigationService NavigationService { get; }
        public ISettingsService SettingsService { get; }

        public bool IsBusy { get; }

        public bool IsInitialized { get; set; }

        Task InitializeAsync();
    }
}
