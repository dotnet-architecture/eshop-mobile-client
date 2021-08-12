using eShopOnContainers.Core.Models.Basket;
using eShopOnContainers.Core.Models.Catalog;
using eShopOnContainers.Core.Services.Basket;
using eShopOnContainers.Core.Services.Settings;
using eShopOnContainers.Core.Services.User;
using eShopOnContainers.Core.ViewModels.Base;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace eShopOnContainers.Core.ViewModels
{
    public class BasketViewModel : ViewModelBase
    {
        private int _badgeCount;
        private ObservableCollection<BasketItem> _basketItems;
        private decimal _total;

        private IBasketService _basketService;
        private ISettingsService _settingsService;
        private IUserService _userService;

        public BasketViewModel()
        {
            _basketService = DependencyService.Get<IBasketService> ();
            _settingsService = DependencyService.Get<ISettingsService>();
            _userService = DependencyService.Get<IUserService> ();
        }

        public int BadgeCount
        {
            get => _badgeCount;
            set
            {
                _badgeCount = value;
                RaisePropertyChanged(() => BadgeCount);
            }
        }

        public ObservableCollection<BasketItem> BasketItems
        {
            get => _basketItems;
            set
            {
                _basketItems = value;
                RaisePropertyChanged(() => BasketItems);
            }
        }

        public decimal Total
        {
            get => _total;
            set
            {
                _total = value;
                RaisePropertyChanged(() => Total);
            }
        }

        public ICommand AddCommand => new Command<BasketItem>(async (item) => await AddItemAsync(item));

        public ICommand DeleteCommand => new Command<BasketItem> (async (item) => await DeleteBasketItemAsync (item));

        public ICommand CheckoutCommand => new Command(async () => await CheckoutAsync());

        public override async Task InitializeAsync (IDictionary<string, string> query)
        {
            if (BasketItems == null)
                BasketItems = new ObservableCollection<BasketItem> ();

            var authToken = _settingsService.AuthAccessToken;
            var userInfo = await _userService.GetUserInfoAsync (authToken);

            // Update Basket
            var basket = await _basketService.GetBasketAsync (userInfo.UserId, authToken);

            if (basket != null && basket.Items != null && basket.Items.Any ())
            {
                BadgeCount = 0;
                BasketItems.Clear ();

                foreach (var basketItem in basket.Items)
                {
                    BadgeCount += basketItem.Quantity;
                    await AddBasketItemAsync (basketItem);
                }
            }

            RaisePropertyChanged (() => BasketItems);
        }

        private async Task AddCatalogItemAsync(CatalogItem item)
        {
            BasketItems.Add(new BasketItem
            {
                ProductId = item.Id,
                ProductName = item.Name,
                PictureUrl = item.PictureUri,
                UnitPrice = item.Price,
                Quantity = 1
            });

            await ReCalculateTotalAsync();
        }

        private async Task AddItemAsync(BasketItem item)
        {
            BadgeCount++;
            await AddBasketItemAsync(item);
            RaisePropertyChanged(() => BasketItems);
        }

        private async Task AddBasketItemAsync(BasketItem item)
        {
            BasketItems.Add(item);
            await ReCalculateTotalAsync();
        }

        private async Task DeleteBasketItemAsync (BasketItem item)
        {
            BasketItems.Remove (item);

            var authToken = _settingsService.AuthAccessToken;
            var userInfo = await _userService.GetUserInfoAsync (authToken);
            var basket = await _basketService.GetBasketAsync (userInfo.UserId, authToken);
            if (basket != null)
            {
                basket.Items.Remove (item);
                await _basketService.UpdateBasketAsync (basket, authToken);
                BadgeCount = basket.Items.Count ();
            }

            await ReCalculateTotalAsync ();
        }

        private async Task ReCalculateTotalAsync()
        {
            Total = 0;

            if (BasketItems == null)
            {
                return;
            }

            foreach (var orderItem in BasketItems)
            {
                Total += (orderItem.Quantity * orderItem.UnitPrice);
            }
        }

        private async Task CheckoutAsync()
        {
            if (BasketItems?.Any() ?? false)
            {
                _basketService.LocalBasketItems = BasketItems;
                await NavigationService.NavigateToAsync ("Checkout");
            }
        }
    }
}