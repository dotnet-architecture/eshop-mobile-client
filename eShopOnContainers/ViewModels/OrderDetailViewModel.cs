using eShopOnContainers.Extensions;
using eShopOnContainers.Models.Orders;
using eShopOnContainers.Services.Order;
using eShopOnContainers.Services.Settings;
using eShopOnContainers.ViewModels.Base;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Maui;
using eShopOnContainers.Services;
using eShopOnContainers.Services.AppEnvironment;
using CommunityToolkit.Mvvm.Input;

namespace eShopOnContainers.ViewModels
{

    [QueryProperty(nameof(OrderNumber), nameof(Models.Orders.Order.OrderNumber))]
    public class OrderDetailViewModel : ViewModelBase
    {
        private readonly ISettingsService _settingsService;
        private readonly IAppEnvironmentService _appEnvironmentService;

        private Order _order;
        private bool _isSubmittedOrder;
        private string _orderStatusText;

        public int OrderNumber { get; set; }

        public Order Order
        {
            get => _order;
            set => SetProperty(ref _order, value);
        }

        public bool IsSubmittedOrder
        {
            get => _isSubmittedOrder;
            set => SetProperty(ref _isSubmittedOrder, value);
        }

        public string OrderStatusText
        {
            get => _orderStatusText;
            set => SetProperty(ref _orderStatusText, value);
        }

        public ICommand ToggleCancelOrderCommand { get; }

        public OrderDetailViewModel(
            IAppEnvironmentService appEnvironmentService,
            IDialogService dialogService, INavigationService navigationService, ISettingsService settingsService)
            : base(dialogService, navigationService, settingsService)
        {
            _appEnvironmentService = appEnvironmentService;
            _settingsService = settingsService;

            ToggleCancelOrderCommand = new AsyncRelayCommand(ToggleCancelOrderAsync);
        }

        public override async Task InitializeAsync ()
        {
            await IsBusyFor(
                async () =>
                {
                    // Get order detail info
                    var authToken = _settingsService.AuthAccessToken;
                    Order = await _appEnvironmentService.OrderService.GetOrderAsync (OrderNumber, authToken);
                    IsSubmittedOrder = Order.OrderStatus == OrderStatus.Submitted;
                    OrderStatusText = Order.OrderStatus.ToString ().ToUpper ();
                });
        }

        private async Task ToggleCancelOrderAsync()
        {
            var authToken = _settingsService.AuthAccessToken;

            var result = await _appEnvironmentService.OrderService.CancelOrderAsync(_order.OrderNumber, authToken);

            if (result)
            {
                OrderStatusText = OrderStatus.Cancelled.ToString().ToUpper();
            }
            else
            {
                Order = await _appEnvironmentService.OrderService.GetOrderAsync(Order.OrderNumber, authToken);
                OrderStatusText = Order.OrderStatus.ToString().ToUpper();
            }

            IsSubmittedOrder = false;
        }
    }
}