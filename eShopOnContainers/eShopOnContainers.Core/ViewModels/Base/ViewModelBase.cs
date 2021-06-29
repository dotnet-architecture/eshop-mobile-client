using eShopOnContainers.Core.Services.Settings;
using eShopOnContainers.Core.Views.Templates;
using eShopOnContainers.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace eShopOnContainers.Core.ViewModels.Base
{
    public abstract class ViewModelBase : ExtendedBindableObject, IQueryAttributable
    {
        protected readonly IDialogService DialogService;
        protected readonly INavigationService NavigationService;

        private bool _isInitialized;

        public bool IsInitialized
        {
            get
            {
                return _isInitialized;
            }

            set
            {
                _isInitialized = value;
                OnPropertyChanged (nameof (IsInitialized));
            }
        }

        private bool _isBusy;

        public bool IsBusy
        {
            get
            {
                return _isBusy;
            }

            set
            {
                _isBusy = value;
                OnPropertyChanged (nameof (IsBusy));
            }
        }

        public ViewModelBase()
        {
            DialogService = ViewModelLocator.Resolve<IDialogService>();
            NavigationService = ViewModelLocator.Resolve<INavigationService>();

            var settingsService = ViewModelLocator.Resolve<ISettingsService>();

            GlobalSetting.Instance.BaseIdentityEndpoint = settingsService.IdentityEndpointBase;
            GlobalSetting.Instance.BaseGatewayShoppingEndpoint = settingsService.GatewayShoppingEndpointBase;
            GlobalSetting.Instance.BaseGatewayMarketingEndpoint = settingsService.GatewayMarketingEndpointBase;
        }

        public virtual Task InitializeAsync (IDictionary<string, string> query)
        {
            return Task.FromResult (false);
        }

        public async void ApplyQueryAttributes (IDictionary<string, string> query)
        {
            //TODO: Remove
            if (query != null)
            {
                foreach (var item in query)
                {
                    System.Diagnostics.Debug.WriteLine ($"{item.Key} - {item.Value}");
                }
            }

            await InitializeAsync (query);
        }
    }
}