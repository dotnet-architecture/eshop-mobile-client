using eShopOnContainers.Models.Orders;
using eShopOnContainers.Services;
using eShopOnContainers.Services.AppEnvironment;
using eShopOnContainers.Services.Settings;
using eShopOnContainers.ViewModels.Base;

namespace eShopOnContainers.ViewModels;

public partial class ProfileViewModel : ViewModelBase
{
    private readonly IAppEnvironmentService _appEnvironmentService;
    private readonly ISettingsService _settingsService;
    private readonly ObservableCollectionEx<Order> _orders;

    [ObservableProperty]
    private Order _selectedOrder;

    public IList<Order> Orders => _orders;

    public ProfileViewModel(
        IAppEnvironmentService appEnvironmentService, ISettingsService settingsService,
        INavigationService navigationService)
        : base(navigationService)
    {
        _appEnvironmentService = appEnvironmentService;
        _settingsService = settingsService;

        _orders = new ObservableCollectionEx<Order>();
    }

    public override async Task InitializeAsync()
    {
        await RefreshAsync();
    }

    [RelayCommand]
    private async Task LogoutAsync()
    {
        await IsBusyFor(
            async () =>
            {
                // Logout
                await NavigationService.NavigateToAsync(
                    "//Login",
                    new Dictionary<string, object> { { "Logout", true } });
            });
    }

    [RelayCommand]
    private async Task RefreshAsync()
    {
        if (IsBusy)
        {
            return;
        }

        await IsBusyFor(
            async () =>
            {
                // Get orders
                var authToken = _settingsService.AuthAccessToken;
                var orders = await _appEnvironmentService.OrderService.GetOrdersAsync(authToken);

                _orders.ReloadData(orders);
            });
    }

    [RelayCommand]
    private async Task OrderDetailAsync(Order order)
    {
        if (order is null)
        {
            return;
        }

        await NavigationService.NavigateToAsync(
            "OrderDetail",
            new Dictionary<string, object> { { nameof(Order.OrderNumber), order.OrderNumber } });
    }
}