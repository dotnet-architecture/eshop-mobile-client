using eShopOnContainers.Models.Basket;
using eShopOnContainers.Models.Catalog;
using eShopOnContainers.Services.Basket;
using eShopOnContainers.Services.Settings;
using eShopOnContainers.Services.User;
using eShopOnContainers.ViewModels.Base;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Maui;
using eShopOnContainers.Services;
using eShopOnContainers.Services.AppEnvironment;
using CommunityToolkit.Mvvm.Input;

namespace eShopOnContainers.ViewModels
{
    public class BasketViewModel : ViewModelBase
    {
        private readonly IAppEnvironmentService _appEnvironmentService;
        private readonly ISettingsService _settingsService;
        private readonly ObservableCollectionEx<BasketItem> _basketItems;

        public int BadgeCount
        {
            get => _basketItems?.Sum(basketItem => basketItem.Quantity) ?? 0;
        }

        public decimal Total
        {
            get => _basketItems?.Sum(basketItem
                => basketItem.Quantity * basketItem.UnitPrice) ?? 0m;
        }

        public ObservableCollectionEx<BasketItem> BasketItems
        {
            get => _basketItems;
        }

        public ICommand AddCommand { get; }

        public ICommand DeleteCommand { get; }

        public ICommand CheckoutCommand { get; }

        public BasketViewModel(
            IAppEnvironmentService appEnvironmentService,
            IDialogService dialogService, INavigationService navigationService, ISettingsService settingsService)
            : base(dialogService, navigationService, settingsService)
        {
            _appEnvironmentService = appEnvironmentService;
            _settingsService = settingsService;

            _basketItems = new ObservableCollectionEx<BasketItem> ();

            AddCommand = new AsyncRelayCommand<BasketItem>(AddBasketItemAsync);
            DeleteCommand = new AsyncRelayCommand<BasketItem>(DeleteBasketItemAsync);
            CheckoutCommand = new AsyncRelayCommand(CheckoutAsync);
        }

        public override async Task InitializeAsync ()
        {
            var authToken = _settingsService.AuthAccessToken;
            var userInfo = await _appEnvironmentService.UserService.GetUserInfoAsync (authToken);

            // Update Basket
            var basket = await _appEnvironmentService.BasketService.GetBasketAsync (userInfo.UserId, authToken);

            if (basket != null && basket.Items != null && basket.Items.Any ())
            {
                await BasketItems.ReloadDataAsync(
                    async innerList =>
                    {
                        foreach (var basketItem in basket.Items.ToArray())
                        {
                            await AddBasketItemAsync (basketItem, innerList);
                        }
                    });
            }
        }

        private void AddCatalogItem(CatalogItem item)
        {
            BasketItems.Add(new BasketItem
            {
                ProductId = item.Id,
                ProductName = item.Name,
                PictureUrl = item.PictureUri,
                UnitPrice = item.Price,
                Quantity = 1
            });

            ReCalculateTotal();
        }

        private Task AddBasketItemAsync(BasketItem item)
        {
            return AddBasketItemAsync(item, BasketItems);
        }

        private async Task AddBasketItemAsync (BasketItem item, IList<BasketItem> basketItems)
        {
            basketItems.Add(item);

            var authToken = _settingsService.AuthAccessToken;
            var userInfo = await _appEnvironmentService.UserService.GetUserInfoAsync(authToken);
            var basket = await _appEnvironmentService.BasketService.GetBasketAsync(userInfo.UserId, authToken);
            if (basket != null)
            {
                basket.Items.Add(item);
                await _appEnvironmentService.BasketService.UpdateBasketAsync(basket, authToken);
            }

            ReCalculateTotal();
        }

        private async Task DeleteBasketItemAsync (BasketItem item)
        {
            BasketItems.Remove (item);

            var authToken = _settingsService.AuthAccessToken;
            var userInfo = await _appEnvironmentService.UserService.GetUserInfoAsync (authToken);
            var basket = await _appEnvironmentService.BasketService.GetBasketAsync (userInfo.UserId, authToken);
            if (basket != null)
            {
                basket.Items.Remove (item);
                await _appEnvironmentService.BasketService.UpdateBasketAsync (basket, authToken);
            }

            ReCalculateTotal ();
        }

        public void ClearBasketItems()
        {
            BasketItems.Clear();

            ReCalculateTotal();
        }

        private void ReCalculateTotal()
        {
            this.OnPropertyChanged(nameof(BadgeCount));
            this.OnPropertyChanged(nameof(Total));
        }

        private async Task CheckoutAsync()
        {
            if (BasketItems?.Any() ?? false)
            {
                _appEnvironmentService.BasketService.LocalBasketItems = BasketItems;
                await NavigationService.NavigateToAsync ("Checkout");
            }
        }
    }
}