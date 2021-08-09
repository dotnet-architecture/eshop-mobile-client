using eShopOnContainers.Core.Models.Basket;
using eShopOnContainers.Core.Models.Catalog;
using eShopOnContainers.Core.Services.Basket;
using eShopOnContainers.Core.Services.Catalog;
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
    public class CatalogViewModel : ViewModelBase
    {
        private ObservableCollection<CatalogItem> _products;
        private CatalogItem _selectedProduct;
        private ObservableCollection<CatalogBrand> _brands;
        private CatalogBrand _brand;
        private ObservableCollection<CatalogType> _types;
        private CatalogType _type;
        private int _badgeCount;
        private ICatalogService _catalogService;
        private IBasketService _basketService;
        private ISettingsService _settingsService;
        private IUserService _userService;

        public CatalogViewModel()
        {
            this.MultipleInitialization = true;

            _catalogService = DependencyService.Get<ICatalogService> ();
            _basketService = DependencyService.Get<IBasketService> ();
            _settingsService = DependencyService.Get<ISettingsService> ();
            _userService = DependencyService.Get<IUserService> ();
        }

        public ObservableCollection<CatalogItem> Products
        {
            get => _products;
            set
            {
                _products = value;
                RaisePropertyChanged(() => Products);
            }
        }

        public CatalogItem SelectedProduct
        {
            get => _selectedProduct;
            set
            {
                if (value == null)
                    return;
                _selectedProduct = null;
                RaisePropertyChanged(() => SelectedProduct);
            }
        }

        public ObservableCollection<CatalogBrand> Brands
        {
            get => _brands;
            set
            {
                _brands = value;
                RaisePropertyChanged(() => Brands);
            }
        }

        public CatalogBrand Brand
        {
            get => _brand;
            set
            {
                _brand = value;
                RaisePropertyChanged(() => Brand);
                RaisePropertyChanged(() => IsFilter);
            }
        }

        public ObservableCollection<CatalogType> Types
        {
            get => _types;
            set
            {
                _types = value;
                RaisePropertyChanged(() => Types);
            }
        }

        public CatalogType Type
        {
            get => _type;
            set
            {
                _type = value;
                RaisePropertyChanged(() => Type);
                RaisePropertyChanged(() => IsFilter);
            }
        }

        public bool IsFilter { get { return Brand != null || Type != null; } }

        public int BadgeCount
        {
            get => _badgeCount;
            set
            {
                _badgeCount = value;
                RaisePropertyChanged(() => BadgeCount);
            }
        }

        public ICommand AddCatalogItemCommand => new Command<CatalogItem>(AddCatalogItem);

        public ICommand FilterCommand => new Command(async () => await FilterAsync());

		public ICommand ClearFilterCommand => new Command(async () => await ClearFilterAsync());

        public ICommand ViewBasketCommand => new Command (async () => await ViewBasket ());

        public override async Task InitializeAsync (IDictionary<string, string> query)
        {
            IsBusy = true;

            // Get Catalog, Brands and Types
            Products = await _catalogService.GetCatalogAsync ();
            Brands = await _catalogService.GetCatalogBrandAsync ();
            Types = await _catalogService.GetCatalogTypeAsync ();

            var authToken = _settingsService.AuthAccessToken;
            var userInfo = await _userService.GetUserInfoAsync (authToken);

            var basket = await _basketService.GetBasketAsync (userInfo.UserId, authToken);

            BadgeCount = basket?.Items?.Count () ?? 0;

            IsBusy = false;
        }

        private async void AddCatalogItem(CatalogItem catalogItem)
        {
            var authToken = _settingsService.AuthAccessToken;
            var userInfo = await _userService.GetUserInfoAsync (authToken);
            var basket = await _basketService.GetBasketAsync (userInfo.UserId, authToken);
            if(basket != null)
            {
                basket.Items.Add (
                    new BasketItem
                    {
                        ProductId = catalogItem.Id,
                        ProductName = catalogItem.Name,
                        PictureUrl = catalogItem.PictureUri,
                        UnitPrice = catalogItem.Price,
                        Quantity = 1
                    });

                await _basketService.UpdateBasketAsync (basket, authToken);
                BadgeCount = basket.Items.Count ();
            }
        }

        private async Task FilterAsync()
        {
            try
            {    
                IsBusy = true;

                if (Brand != null && Type != null)
                {
                    Products = await _catalogService.FilterAsync(Brand.Id, Type.Id);
                }
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task ClearFilterAsync()
        {
            IsBusy = true;

            Brand = null;
            Type = null;
            Products = await _catalogService.GetCatalogAsync();

            IsBusy = false;
        }

        private Task ViewBasket()
        {
            return NavigationService.NavigateToAsync ("Basket");
        }
    }
}