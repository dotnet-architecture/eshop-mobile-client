using System.Diagnostics;
using System.Globalization;
using eShopOnContainers.Services;
using eShopOnContainers.Services.AppEnvironment;
using eShopOnContainers.Services.Location;
using eShopOnContainers.Services.Settings;
using eShopOnContainers.Services.Theme;

namespace eShopOnContainers;

public partial class App : Application
{
    private readonly ISettingsService _settingsService;
    private readonly IAppEnvironmentService _appEnvironmentService;
    private readonly INavigationService _navigationService;
    private readonly ILocationService _locationService;
    private readonly ITheme _theme;

    public App(
        ISettingsService settingsService, IAppEnvironmentService appEnvironmentService,
        INavigationService navigationService, ILocationService locationService,
        ITheme theme)
    {
        _settingsService = settingsService;
        _appEnvironmentService = appEnvironmentService;
        _navigationService = navigationService;
        _locationService = locationService;
        _theme = theme;

        InitializeComponent();

        InitApp();

        MainPage = new AppShell(navigationService);
    }

    private void InitApp()
    {
        if (VersionTracking.IsFirstLaunchEver)
        {
            _settingsService.UseMocks = true;
        }

        if (!_settingsService.UseMocks)
        {
            _appEnvironmentService.UpdateDependencies(_settingsService.UseMocks);
        }
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

        OnResume();
    }

    protected override void OnSleep()
    {
        SetStatusBar();
        RequestedThemeChanged -= App_RequestedThemeChanged;
    }

    protected override void OnResume()
    {
        SetStatusBar();
        RequestedThemeChanged += App_RequestedThemeChanged;
    }

    private void App_RequestedThemeChanged(object sender, AppThemeChangedEventArgs e)
    {
        Dispatcher.Dispatch(() => SetStatusBar());
    }

    void SetStatusBar()
    {
        var nav = Current.MainPage as NavigationPage;

        if (Current.RequestedTheme == AppTheme.Dark)
        {
            _theme?.SetStatusBarColor(Colors.Black, false);
            if (nav != null)
            {
                nav.BarBackgroundColor = Colors.Black;
                nav.BarTextColor = Colors.White;
            }
        }
        else
        {
            _theme?.SetStatusBarColor(Colors.White, true);
            if (nav != null)
            {
                nav.BarBackgroundColor = Colors.White;
                nav.BarTextColor = Colors.Black;
            }
        }
    }

    private async Task GetGpsLocation()
    {
        try
        {
            var request = new GeolocationRequest(GeolocationAccuracy.High);
            var location = await Geolocation.GetLocationAsync(request, CancellationToken.None).ConfigureAwait(false);

            if (location != null)
            {
                _settingsService.Latitude = location.Latitude.ToString();
                _settingsService.Longitude = location.Longitude.ToString();
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
        var location = new Models.Location.Location
        {
            Latitude = double.Parse(_settingsService.Latitude, CultureInfo.InvariantCulture),
            Longitude = double.Parse(_settingsService.Longitude, CultureInfo.InvariantCulture)
        };

        await _locationService.UpdateUserLocation(location, _settingsService.AuthAccessToken);
    }
}