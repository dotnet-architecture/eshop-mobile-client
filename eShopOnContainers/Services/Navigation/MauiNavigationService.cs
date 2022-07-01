using eShopOnContainers.Services.Settings;

namespace eShopOnContainers.Services;

public class MauiNavigationService : INavigationService
{
    private readonly ISettingsService _settingsService;

    public MauiNavigationService(ISettingsService settingsService)
    {
        _settingsService = settingsService;
    }

    public Task InitializeAsync()
    {
        if (string.IsNullOrEmpty(_settingsService.AuthAccessToken))
            return NavigateToAsync("//Login");
        else
            return NavigateToAsync("//Main/Catalog");
    }

    public Task NavigateToAsync (string route, IDictionary<string, object> routeParameters = null)
    {
        var shellNavigation = new ShellNavigationState(route);

        return
            routeParameters != null
                ? Shell.Current.GoToAsync(shellNavigation, routeParameters)
                : Shell.Current.GoToAsync(shellNavigation);
    }

    public async Task PopAsync()
    {
        await Shell.Current.GoToAsync("..");
    }
}