using System;
using CommunityToolkit.Mvvm.ComponentModel;
using eShopOnContainers.Services;
using eShopOnContainers.Services.Settings;

namespace eShopOnContainers.ViewModels.Base
{
    public abstract class ViewModelBase : ObservableObject, IViewModelBase
    {
        private readonly SemaphoreSlim _isBusyLock = new SemaphoreSlim(1, 1);

        private bool _isInitialized;
        private bool _isBusy;

        public IDialogService DialogService { get; private set; }

        public INavigationService NavigationService { get; private set; }

        public ISettingsService SettingsService { get; private set; }

        public bool IsInitialized
        {
            get => _isInitialized;
            set => SetProperty(ref _isInitialized, value);
        }

        public bool IsBusy
        {
            get => _isBusy;
            private set => SetProperty(ref _isBusy, value);
        }

        public ViewModelBase(IDialogService dialogService, INavigationService navigationService, ISettingsService settingsService)
        {
            DialogService = dialogService;
            NavigationService = navigationService;
            SettingsService = settingsService;

            GlobalSetting.Instance.BaseIdentityEndpoint = SettingsService.IdentityEndpointBase;
            GlobalSetting.Instance.BaseGatewayShoppingEndpoint = SettingsService.GatewayShoppingEndpointBase;
            GlobalSetting.Instance.BaseGatewayMarketingEndpoint = SettingsService.GatewayMarketingEndpointBase;
        }

        public virtual void ApplyQueryAttributes(IDictionary<string, object> query)
        {
        }

        public virtual Task InitializeAsync()
        {
            return Task.CompletedTask;
        }

        public async Task IsBusyFor(Func<Task> unitOfWork)
        {
            await _isBusyLock.WaitAsync();

            try
            {
                IsBusy = true;

                await unitOfWork();
            }
            finally
            {
                IsBusy = false;
                _isBusyLock.Release();
            }
        }
    }
}

