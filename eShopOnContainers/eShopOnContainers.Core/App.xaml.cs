using eShopOnContainers.Core;
using eShopOnContainers.Core.Models.Location;
using eShopOnContainers.Core.Services.Dependency;
using eShopOnContainers.Core.Services.Location;
using eShopOnContainers.Core.Services.Settings;
using eShopOnContainers.Core.ViewModels.Base;
using eShopOnContainers.Services;
using System;
using System.Diagnostics;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace eShopOnContainers
{
    public partial class App : Application
    {
        ISettingsService _settingsService;

        public App()
        {
            InitializeComponent();

            InitApp();

            MainPage = new AppShell ();
        }

        private void InitApp()
        {
            _settingsService = ViewModelLocator.Resolve<ISettingsService>();
            if (!_settingsService.UseMocks)
                ViewModelLocator.UpdateDependencies(_settingsService.UseMocks);
        }

        private Task InitNavigation()
        {
            var navigationService = ViewModelLocator.Resolve<INavigationService>();
            return navigationService.InitializeAsync();
        }

        protected override async void OnStart()
        {
            base.OnStart();

            if (_settingsService.AllowGpsLocation && !_settingsService.UseFakeLocation)
            {
                await GetGpsLocation();
            }
            if (!_settingsService.UseMocks && !string.IsNullOrEmpty(_settingsService.AuthAccessToken))
            {
                await SendCurrentLocation();
            }

            base.OnResume();
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        private async Task GetGpsLocation()
        {
            try
            {
                var request = new GeolocationRequest (GeolocationAccuracy.High);
                var location = await Geolocation.GetLocationAsync (request, CancellationToken.None).ConfigureAwait(false);

                if (location != null)
                {
                    _settingsService.Latitude = location.Latitude.ToString ();
                    _settingsService.Longitude = location.Longitude.ToString ();
                }
            }
            catch (Exception ex)
            {
                if (ex is FeatureNotEnabledException || ex is FeatureNotEnabledException || ex is PermissionException)
                {
                    _settingsService.AllowGpsLocation = false;
                }

                // Unable to get location
                Debug.WriteLine(ex);
            }
        }

        private async Task SendCurrentLocation()
        {
            var location = new Core.Models.Location.Location
            {
                Latitude = double.Parse(_settingsService.Latitude, CultureInfo.InvariantCulture),
                Longitude = double.Parse(_settingsService.Longitude, CultureInfo.InvariantCulture)
            };

            var locationService = ViewModelLocator.Resolve<ILocationService>();
            await locationService.UpdateUserLocation(location, _settingsService.AuthAccessToken);
        }
    }
}