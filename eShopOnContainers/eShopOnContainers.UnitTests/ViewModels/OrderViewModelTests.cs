using eShopOnContainers.Core;
using eShopOnContainers.Core.Models.Orders;
using eShopOnContainers.Core.Services.Order;
using eShopOnContainers.Core.Services.Settings;
using eShopOnContainers.Core.ViewModels;
using eShopOnContainers.Core.ViewModels.Base;
using eShopOnContainers.UnitTests.Mocks;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace eShopOnContainers.UnitTests
{
    public class OrderViewModelTests
    {
        public OrderViewModelTests()
        {
            ViewModelLocator.UpdateDependencies(true);
        }

        [Fact]
        public void OrderPropertyIsNullWhenViewModelInstantiatedTest()
        {
            Xamarin.Forms.DependencyService.RegisterSingleton<ISettingsService>(new MockSettingsService());
            Xamarin.Forms.DependencyService.RegisterSingleton<IOrderService>(new OrderMockService());
            var orderViewModel = new OrderDetailViewModel();

            Assert.Null(orderViewModel.Order);
        }

        [Fact]
        public async Task OrderPropertyIsNotNullAfterViewModelInitializationTest()
        {
            Xamarin.Forms.DependencyService.RegisterSingleton<ISettingsService>(new MockSettingsService());
            var orderService = new OrderMockService();
            Xamarin.Forms.DependencyService.RegisterSingleton<IOrderService>(orderService);
            var orderViewModel = new OrderDetailViewModel();

            var order = await orderService.GetOrderAsync(1, GlobalSetting.Instance.AuthToken);
            await orderViewModel.InitializeAsync(new Dictionary<string, string> { { nameof(Order.OrderNumber), order.OrderNumber.ToString() } });

            Assert.NotNull(orderViewModel.Order);
        }

        [Fact]
        public async Task SettingOrderPropertyShouldRaisePropertyChanged()
        {
            bool invoked = false;
            Xamarin.Forms.DependencyService.RegisterSingleton<ISettingsService>(new MockSettingsService());
            var orderService = new OrderMockService();
            Xamarin.Forms.DependencyService.RegisterSingleton<IOrderService>(orderService);
            var orderViewModel = new OrderDetailViewModel();

            orderViewModel.PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName.Equals("Order"))
                    invoked = true;
            };
            var order = await orderService.GetOrderAsync(1, GlobalSetting.Instance.AuthToken);
            await orderViewModel.InitializeAsync(new Dictionary<string, string> { { nameof(Order.OrderNumber), order.OrderNumber.ToString() } });

            Assert.True(invoked);
        }
    }
}