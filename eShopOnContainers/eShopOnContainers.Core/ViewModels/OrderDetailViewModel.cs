using eShopOnContainers.Core.Extensions;
using eShopOnContainers.Core.Models.Orders;
using eShopOnContainers.Core.Services.Order;
using eShopOnContainers.Core.Services.Settings;
using eShopOnContainers.Core.ViewModels.Base;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace eShopOnContainers.Core.ViewModels
{
    public class OrderDetailViewModel : ViewModelBase
    {
        private readonly ISettingsService _settingsService;
        private readonly IOrderService _orderService;

        private Order _order;
        private bool _isSubmittedOrder;
        private string _orderStatusText;

        public OrderDetailViewModel()
        {
            _settingsService = DependencyService.Get<ISettingsService> ();
            _orderService = DependencyService.Get<IOrderService> ();
        }

        public Order Order
        {
            get => _order;
            set
            {
                _order = value;
                RaisePropertyChanged(() => Order);
            }
        }

        public bool IsSubmittedOrder
        {
            get => _isSubmittedOrder;
            set
            {
                _isSubmittedOrder = value;
                RaisePropertyChanged(() => IsSubmittedOrder);
            }
        }

        public string OrderStatusText
        {
            get => _orderStatusText;
            set
            {
                _orderStatusText = value;
                RaisePropertyChanged(() => OrderStatusText);
            }
        }


        public ICommand ToggleCancelOrderCommand => new Command(async () => await ToggleCancelOrderAsync());

        public override async Task InitializeAsync (IDictionary<string, string> query)
        {
            var orderNumber = query.GetValueAsInt (nameof (Order.OrderNumber));

            if (orderNumber.ContainsKeyAndValue)
            {
                IsBusy = true;

                // Get order detail info
                var authToken = _settingsService.AuthAccessToken;
                Order = await _orderService.GetOrderAsync (orderNumber.Value, authToken);
                IsSubmittedOrder = Order.OrderStatus == OrderStatus.Submitted;
                OrderStatusText = Order.OrderStatus.ToString ().ToUpper ();

                IsBusy = false;
            }
        }

        private async Task ToggleCancelOrderAsync()
        {
            var authToken = _settingsService.AuthAccessToken;

            var result = await _orderService.CancelOrderAsync(_order.OrderNumber, authToken);

            if (result)
            {
                OrderStatusText = OrderStatus.Cancelled.ToString().ToUpper();
            }
            else
            {
                Order = await _orderService.GetOrderAsync(Order.OrderNumber, authToken);
                OrderStatusText = Order.OrderStatus.ToString().ToUpper();
            }

            IsSubmittedOrder = false;
        }
    }
}