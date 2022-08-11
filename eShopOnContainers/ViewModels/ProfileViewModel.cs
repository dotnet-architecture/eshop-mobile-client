using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using eShopOnContainers.Models.Orders;
using eShopOnContainers.Services;
using eShopOnContainers.Services.AppEnvironment;
using eShopOnContainers.Services.Settings;
using eShopOnContainers.ViewModels.Base;

namespace eShopOnContainers.ViewModels;

public class ProfileViewModel : ViewModelBase
{
    private readonly IAppEnvironmentService _appEnvironmentService;
    private readonly ObservableCollectionEx<Order> _orders;

    private Order _selectedOrder;

    public IList<Order> Orders => _orders;

    public Order SelectedOrder
    {
        get => _selectedOrder;
        set => SetProperty(ref _selectedOrder, value);
    }

    public ICommand RefreshCommand { get; }
    public ICommand LogoutCommand { get; }
    public ICommand OrderDetailCommand { get; }

    public ProfileViewModel(
        IAppEnvironmentService appEnvironmentService,
        IDialogService dialogService, INavigationService navigationService, ISettingsService settingsService)
        : base(dialogService, navigationService, settingsService)
    {
        _appEnvironmentService = appEnvironmentService;

        _orders = new ObservableCollectionEx<Order>();

        RefreshCommand = new AsyncRelayCommand(LoadOrdersAsync);
        LogoutCommand = new AsyncRelayCommand(LogoutAsync);
        OrderDetailCommand = new AsyncRelayCommand<Order>(OrderDetailAsync);
    }

    public override async Task InitializeAsync()
    {
        await LoadOrdersAsync();
    }

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

    private async Task LoadOrdersAsync()
    {
        if (IsBusy)
        {
            return;
        }

        await IsBusyFor(
            async () =>
            {
                // Get orders
                var authToken = SettingsService.AuthAccessToken;
                var orders = await _appEnvironmentService.OrderService.GetOrdersAsync(authToken);

                _orders.ReloadData(orders);
            });
    }

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