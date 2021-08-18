using eShopOnContainers.Core.Models.Location;
using eShopOnContainers.Core.Models.User;
using eShopOnContainers.Core.Services.Location;
using eShopOnContainers.Core.Services.Settings;
using eShopOnContainers.Core.ViewModels.Base;
using System.Globalization;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using eShopOnContainers.Core.Services.Dependency;
using Xamarin.Essentials;
using System.Runtime.CompilerServices;

namespace eShopOnContainers.Core.ViewModels
{
    public class SettingsViewModel : ViewModelBase
    {
        private bool _useAzureServices;
        private bool _allowGpsLocation;
        private bool _useFakeLocation;
        private string _identityEndpoint;
        private string _gatewayShoppingEndpoint;
        private string _gatewayMarketingEndpoint;
        private double _latitude;
        private double _longitude;
        private string _gpsWarningMessage;

        private readonly ISettingsService _settingsService;
        private readonly ILocationService _locationService;

        public SettingsViewModel()
        {
            _settingsService = Xamarin.Forms.DependencyService.Get<ISettingsService> ();
            _locationService = Xamarin.Forms.DependencyService.Get<ILocationService> ();

            _useAzureServices = !_settingsService.UseMocks;
            _identityEndpoint = _settingsService.IdentityEndpointBase;
            _gatewayShoppingEndpoint = _settingsService.GatewayShoppingEndpointBase;
            _gatewayMarketingEndpoint = _settingsService.GatewayMarketingEndpointBase;
            _latitude = double.Parse(_settingsService.Latitude, CultureInfo.CurrentCulture);
            _longitude = double.Parse(_settingsService.Longitude, CultureInfo.CurrentCulture);
            _useFakeLocation = _settingsService.UseFakeLocation;
            _allowGpsLocation = _settingsService.AllowGpsLocation;
            _gpsWarningMessage = string.Empty;
        }

        public string TitleUseAzureServices
        {
            get => "Use Microservices/Containers from eShopOnContainers";
        }

        public string DescriptionUseAzureServices
        {
            get
            {
                return !UseAzureServices
                    ? "Currently using mock services that are simulated objects that mimic the behavior of real services using a controlled approach. Toggle on to configure the use of microserivces/containers."
                        : "When enabling the use of microservices/containers, the app will attempt to use real services deployed as Docker/Kubernetes containers at the specified base endpoint, which will must be reachable through the network.";
            }
        }

        public bool UseAzureServices
        {
            get => _useAzureServices;
            set
            {
                _useAzureServices = value;
                UpdateUseAzureServices();
                RaisePropertyChanged(() => UseAzureServices);
            }
        }

        public string TitleUseFakeLocation
        {
            get { return !UseFakeLocation ? "Use Real Location" : "Use Fake Location"; }
        }

        public string DescriptionUseFakeLocation
        {
            get
            {
                return !UseFakeLocation
                    ? "When enabling location, the app will attempt to use the location from the device."
                        : "Fake Location data is added for marketing campaign testing.";
            }
        }

        public bool UseFakeLocation
        {
            get => _useFakeLocation;
            set
            {
                _useFakeLocation = value;
                UpdateFakeLocation();
                RaisePropertyChanged(() => UseFakeLocation);
            }
        }

        public string TitleAllowGpsLocation
        {
            get { return !AllowGpsLocation ? "GPS Location Disabled" : "GPS Location Enabled"; }
        }

        public string DescriptionAllowGpsLocation
        {
            get
            {
                return !AllowGpsLocation
                    ? "When disabling location, you won't receive location campaigns based upon your location."
                        : "When enabling location, you'll receive location campaigns based upon your location.";
            }
        }

        public string GpsWarningMessage
        {
            get => _gpsWarningMessage;
            set
            {
                _gpsWarningMessage = value;
                RaisePropertyChanged(() => GpsWarningMessage);
            }
        }

        public string IdentityEndpoint
        {
            get => _identityEndpoint;
            set
            {
                _identityEndpoint = value;
                if (!string.IsNullOrEmpty(_identityEndpoint))
                {
                    UpdateIdentityEndpoint();
                }
                RaisePropertyChanged(() => IdentityEndpoint);
            }
        }

        public string GatewayShoppingEndpoint
        {
            get => _gatewayShoppingEndpoint;
            set
            {
                _gatewayShoppingEndpoint = value;
                if (!string.IsNullOrEmpty(_gatewayShoppingEndpoint))
                {
                    UpdateGatewayShoppingEndpoint();
                }
                RaisePropertyChanged(() => GatewayShoppingEndpoint);
            }
        }

        public string GatewayMarketingEndpoint
        {
            get => _gatewayMarketingEndpoint;
            set
            {
                _gatewayMarketingEndpoint = value;
                if (!string.IsNullOrEmpty(_gatewayMarketingEndpoint))
                {
                    UpdateGatewayMarketingEndpoint();
                }
                RaisePropertyChanged(() => GatewayMarketingEndpoint);
            }
        }

        public double Latitude
        {
            get => _latitude;
            set
            {
                _latitude = value;
                UpdateLatitude();
                RaisePropertyChanged(() => Latitude);
            }
        }

        public double Longitude
        {
            get => _longitude;
            set
            {
                _longitude = value;
                UpdateLongitude();
                RaisePropertyChanged(() => Longitude);
            }
        }

        public bool AllowGpsLocation
        {
            get => _allowGpsLocation;
            set
            {
                _allowGpsLocation = value;
                RaisePropertyChanged(() => AllowGpsLocation);
            }
        }

        public bool UserIsLogged => !string.IsNullOrEmpty(_settingsService.AuthAccessToken);

        public ICommand ToggleMockServicesCommand => new Command(async () => await ToggleMockServicesAsync());

        public ICommand ToggleFakeLocationCommand => new Command(ToggleFakeLocationAsync);

        public ICommand ToggleSendLocationCommand => new Command(async () => await ToggleSendLocationAsync());

        public ICommand ToggleAllowGpsLocationCommand => new Command(ToggleAllowGpsLocation);

        protected override async void OnPropertyChanged ([CallerMemberName] string propertyName = null)
        {
            base.OnPropertyChanged (propertyName);

            if (propertyName == nameof (AllowGpsLocation))
            {
                await UpdateAllowGpsLocation ();
            }
        }

        private async Task ToggleMockServicesAsync()
        {
            ViewModelLocator.UpdateDependencies(!UseAzureServices);
            RaisePropertyChanged(() => TitleUseAzureServices);
            RaisePropertyChanged(() => DescriptionUseAzureServices);

            //TODO: We should re-evaluate this workflow
            if (UseAzureServices)
            {
                _settingsService.AuthAccessToken = string.Empty;
                _settingsService.AuthIdToken = string.Empty;
            }
        }

        private void ToggleFakeLocationAsync()
        {
            ViewModelLocator.UpdateDependencies(!UseAzureServices);
            RaisePropertyChanged(() => TitleUseFakeLocation);
            RaisePropertyChanged(() => DescriptionUseFakeLocation);
        }

        private async Task ToggleSendLocationAsync()
        {
            if (!_settingsService.UseMocks)
            {
                var locationRequest = new Models.Location.Location
                {
                    Latitude = _latitude,
                    Longitude = _longitude
                };
                var authToken = _settingsService.AuthAccessToken;

                await _locationService.UpdateUserLocation(locationRequest, authToken);
            }
        }

        private void ToggleAllowGpsLocation()
        {
            RaisePropertyChanged(() => TitleAllowGpsLocation);
            RaisePropertyChanged(() => DescriptionAllowGpsLocation);
        }

        private void UpdateUseAzureServices()
        {
            // Save use mocks services to local storage
            _settingsService.UseMocks = !_useAzureServices;
        }

        private void UpdateIdentityEndpoint()
        {
            // Update remote endpoint (save to local storage)
            GlobalSetting.Instance.BaseIdentityEndpoint = _settingsService.IdentityEndpointBase = _identityEndpoint;
        }

        private void UpdateGatewayShoppingEndpoint()
        {
            GlobalSetting.Instance.BaseGatewayShoppingEndpoint = _settingsService.GatewayShoppingEndpointBase = _gatewayShoppingEndpoint;
        }

        private void UpdateGatewayMarketingEndpoint()
        {
            GlobalSetting.Instance.BaseGatewayMarketingEndpoint = _settingsService.GatewayMarketingEndpointBase = _gatewayMarketingEndpoint;
        }

        private void UpdateFakeLocation()
        {
            _settingsService.UseFakeLocation = _useFakeLocation;
        }

        private void UpdateLatitude()
        {
            // Update fake latitude (save to local storage)
            _settingsService.Latitude = _latitude.ToString();
        }

        private void UpdateLongitude()
        {
            // Update fake longitude (save to local storage)
            _settingsService.Longitude = _longitude.ToString();
        }

        private async Task UpdateAllowGpsLocation()
        {
            if (_allowGpsLocation)
            {
                bool hasWhenInUseLocationPermissions;
                bool hasBackgroundLocationPermissions;

                if (await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>() != PermissionStatus.Granted)
                {
                    hasWhenInUseLocationPermissions = await Permissions.RequestAsync<Permissions.LocationWhenInUse> () == PermissionStatus.Granted;
                }
                else
                {
                    hasWhenInUseLocationPermissions = true;
                }

                if (await Permissions.CheckStatusAsync<Permissions.LocationAlways> () != PermissionStatus.Granted)
                {
                    hasBackgroundLocationPermissions = await Permissions.RequestAsync<Permissions.LocationAlways> () == PermissionStatus.Granted;
                }
                else
                {
                    hasBackgroundLocationPermissions = true;
                }


                if (!hasWhenInUseLocationPermissions || !hasBackgroundLocationPermissions)
                {
                    _allowGpsLocation = false;
                    GpsWarningMessage = "Enable the GPS sensor on your device";
                }
                else
                {
                    _settingsService.AllowGpsLocation = _allowGpsLocation;
                    GpsWarningMessage = string.Empty;
                }
            }
            else
            {
                _settingsService.AllowGpsLocation = _allowGpsLocation;
            }
        }
    }
}