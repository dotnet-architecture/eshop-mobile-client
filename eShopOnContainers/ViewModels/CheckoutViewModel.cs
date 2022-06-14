using eShopOnContainers.Models.Basket;
using eShopOnContainers.Models.Navigation;
using eShopOnContainers.Models.Orders;
using eShopOnContainers.Models.User;
using eShopOnContainers.Services.Basket;
using eShopOnContainers.Services.Order;
using eShopOnContainers.Services.Settings;
using eShopOnContainers.Services.User;
using eShopOnContainers.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Maui;
using eShopOnContainers.Services;
using eShopOnContainers.Services.AppEnvironment;
using CommunityToolkit.Mvvm.Input;

namespace eShopOnContainers.ViewModels
{
    public class CheckoutViewModel : ViewModelBase
    {
        private readonly ISettingsService _settingsService;
        private readonly IAppEnvironmentService _appEnvironmentService;

        private readonly BasketViewModel _basketViewModel;

        private Order _order;
        private Address _shippingAddress;

        public Order Order
        {
            get => _order;
            set => SetProperty(ref _order, value);
        }

        public Address ShippingAddress
        {
            get => _shippingAddress;
            set => SetProperty(ref _shippingAddress, value);
        }

        public ICommand CheckoutCommand { get; }

        public CheckoutViewModel(
            IAppEnvironmentService appEnvironmentService,
            IDialogService dialogService, INavigationService navigationService, ISettingsService settingsService,
            BasketViewModel basketViewModel)
            : base(dialogService, navigationService, settingsService)
        {
            _appEnvironmentService = appEnvironmentService;
            _settingsService = settingsService;

            _basketViewModel = basketViewModel;

            CheckoutCommand = new AsyncRelayCommand(CheckoutAsync);
        }       

        public override async Task InitializeAsync ()
        {
            await IsBusyFor(
                async () =>
                {
                    var basketItems = _appEnvironmentService.BasketService.LocalBasketItems;

                    var authToken = _settingsService.AuthAccessToken;
                    var userInfo = await _appEnvironmentService.UserService.GetUserInfoAsync (authToken);

                    // Create Shipping Address
                    ShippingAddress = new Address
                    {
                        Id = !string.IsNullOrEmpty (userInfo?.UserId) ? new Guid (userInfo.UserId) : Guid.NewGuid (),
                        Street = userInfo?.Street,
                        ZipCode = userInfo?.ZipCode,
                        State = userInfo?.State,
                        Country = userInfo?.Country,
                        City = userInfo?.Address
                    };

                    // Create Payment Info
                    var paymentInfo = new PaymentInfo
                    {
                        CardNumber = userInfo?.CardNumber,
                        CardHolderName = userInfo?.CardHolder,
                        CardType = new CardType { Id = 3, Name = "MasterCard" },
                        SecurityNumber = userInfo?.CardSecurityNumber
                    };

                    var orderItems = CreateOrderItems (basketItems);

                    // Create new Order
                    Order = new Order
                    {
                        BuyerId = userInfo.UserId,
                        OrderItems = orderItems,
                        OrderStatus = OrderStatus.Submitted,
                        OrderDate = DateTime.Now,
                        CardHolderName = paymentInfo.CardHolderName,
                        CardNumber = paymentInfo.CardNumber,
                        CardSecurityNumber = paymentInfo.SecurityNumber,
                        CardExpiration = DateTime.Now.AddYears (5),
                        CardTypeId = paymentInfo.CardType.Id,
                        ShippingState = _shippingAddress.State,
                        ShippingCountry = _shippingAddress.Country,
                        ShippingStreet = _shippingAddress.Street,
                        ShippingCity = _shippingAddress.City,
                        ShippingZipCode = _shippingAddress.ZipCode,
                        Total = CalculateTotal (orderItems),
                    };

                    if (_settingsService.UseMocks)
                    {
                        // Get number of orders
                        var orders = await _appEnvironmentService.OrderService.GetOrdersAsync (authToken);

                        // Create the OrderNumber
                        Order.OrderNumber = orders.Count() + 1;
                        OnPropertyChanged (nameof(Order));
                    }
                });
        }

        private async Task CheckoutAsync()
        {
            try
            {
                var authToken = _settingsService.AuthAccessToken;

                var basket = _appEnvironmentService.OrderService.MapOrderToBasket(Order);
                basket.RequestId = Guid.NewGuid();

                // Create basket checkout
                await _appEnvironmentService.BasketService.CheckoutAsync(basket, authToken);

                if (_settingsService.UseMocks)
                {
                    await _appEnvironmentService.OrderService.CreateOrderAsync(Order, authToken);
                }

                // Clean Basket
                await _appEnvironmentService.BasketService.ClearBasketAsync(_shippingAddress.Id.ToString(), authToken);

                // Reset Basket badge
                _basketViewModel.ClearBasketItems();

                // Navigate to Orders
                await NavigationService.NavigateToAsync("//Main/Catalog");

                // Show Dialog
                await DialogService.ShowAlertAsync("Order sent successfully!", "Checkout", "Ok");
            }
            catch
            {
                await DialogService.ShowAlertAsync("An error ocurred. Please, try again.", "Oops!", "Ok");
            }
        }

        private List<OrderItem> CreateOrderItems(IEnumerable<BasketItem> basketItems)
        {
            var orderItems = new List<OrderItem>();

            foreach (var basketItem in basketItems)
            {
                if (!string.IsNullOrEmpty(basketItem.ProductName))
                {
                    orderItems.Add(new OrderItem
                    {
                        OrderId = null,
                        ProductId = basketItem.ProductId,
                        ProductName = basketItem.ProductName,
                        PictureUrl = basketItem.PictureUrl,
                        Quantity = basketItem.Quantity,
                        UnitPrice = basketItem.UnitPrice
                    });
                }
            }

            return orderItems;
        }

        private decimal CalculateTotal(List<OrderItem> orderItems)
        {
            decimal total = 0;

            foreach (var orderItem in orderItems)
            {
                total += (orderItem.Quantity * orderItem.UnitPrice);
            }

            return total;
        }
    }
}