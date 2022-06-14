using eShopOnContainers.Models.Basket;
using eShopOnContainers.Models.Catalog;
using eShopOnContainers.Services.Basket;
using eShopOnContainers.Services.Catalog;
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
using eShopOnContainers.Extensions;
using CommunityToolkit.Mvvm.Input;

namespace eShopOnContainers.ViewModels
{
    public class CatalogViewModel : ViewModelBase
    {
        private readonly IAppEnvironmentService _appEnvironmentService;
        private readonly ISettingsService _settingsService;

        private readonly ObservableCollectionEx<CatalogItem> _products;
        private readonly ObservableCollectionEx<CatalogBrand> _brands;
        private readonly ObservableCollectionEx<CatalogType> _types;

        private CatalogItem _selectedProduct;
        private CatalogBrand _brand;
        private CatalogType _type;
        private int _badgeCount;

        public IList<CatalogItem> Products
        {
            get => _products;
        }

        public CatalogItem SelectedProduct
        {
            get => _selectedProduct;
            set => SetProperty(ref _selectedProduct, value);
        }

        public IEnumerable<CatalogBrand> Brands
        {
            get => _brands;
        }

        public CatalogBrand Brand
        {
            get => _brand;
            set
            {
                SetProperty(ref _brand, value);
                this.OnPropertyChanged(nameof(IsFilter));
            }
        }

        public IEnumerable<CatalogType> Types
        {
            get => _types;
        }

        public CatalogType Type
        {
            get => _type;
            set
            {
                SetProperty(ref _type, value);
                this.OnPropertyChanged(nameof(IsFilter));
            }
        }

        public int BadgeCount
        {
            get => _badgeCount;
            set => SetProperty(ref _badgeCount, value);
        }

        public bool IsFilter
        {
            get => Brand != null || Type != null;
        }

        public ICommand AddCatalogItemCommand { get; }

        public ICommand FilterCommand { get; }

        public ICommand ClearFilterCommand { get; }

        public ICommand ViewBasketCommand { get; }

        public CatalogViewModel(
            IAppEnvironmentService appEnvironmentService,
            IDialogService dialogService, INavigationService navigationService, ISettingsService settingsService)
            : base(dialogService, navigationService, settingsService)
        {
            _appEnvironmentService = appEnvironmentService;
            _settingsService = settingsService;

            _products = new ObservableCollectionEx<CatalogItem>();
            _brands = new ObservableCollectionEx<CatalogBrand>();
            _types = new ObservableCollectionEx<CatalogType>();

            AddCatalogItemCommand = new RelayCommand<CatalogItem>(AddCatalogItem);

            FilterCommand = new AsyncRelayCommand(FilterAsync);

            ClearFilterCommand = new AsyncRelayCommand(ClearFilterAsync);

            ViewBasketCommand = new AsyncRelayCommand(ViewBasket);
        }

        public override async Task InitializeAsync ()
        {
            await IsBusyFor(
                async () =>
                {
                    // Get Catalog, Brands and Types
                    var products = await _appEnvironmentService.CatalogService.GetCatalogAsync ();
                    var brands = await _appEnvironmentService.CatalogService.GetCatalogBrandAsync ();
                    var types = await _appEnvironmentService.CatalogService.GetCatalogTypeAsync ();

                    var authToken = _settingsService.AuthAccessToken;
                    var userInfo = await _appEnvironmentService.UserService.GetUserInfoAsync (authToken);

                    var basket = await _appEnvironmentService.BasketService.GetBasketAsync (userInfo.UserId, authToken);

                    BadgeCount = basket?.Items?.Count () ?? 0;

                    _products.ReloadData(products);
                    _brands.ReloadData(brands);
                    _types.ReloadData(types);
                });
        }

        private async void AddCatalogItem(CatalogItem catalogItem)
        {
            if(catalogItem is null)
            {
                return;
            }

            var authToken = _settingsService.AuthAccessToken;
            var userInfo = await _appEnvironmentService.UserService.GetUserInfoAsync (authToken);
            var basket = await _appEnvironmentService.BasketService.GetBasketAsync (userInfo.UserId, authToken);
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

                await _appEnvironmentService.BasketService.UpdateBasketAsync (basket, authToken);
                BadgeCount = basket.Items.Count ();
            }

            SelectedProduct = null;
        }

        private async Task FilterAsync()
        {
            await IsBusyFor(
                async () =>
                {
                    if (Brand != null || Type != null)
                    {
                        var filteredProducts = await _appEnvironmentService.CatalogService.FilterAsync(Brand.Id, Type.Id);
                        _products.ReloadData(filteredProducts);
                    }

                    await NavigationService.PopAsync();
                });
        }

        private async Task ClearFilterAsync()
        {
            await IsBusyFor(
                async () =>
                {
                    Brand = null;
                    Type = null;
                    var allProducts = await _appEnvironmentService.CatalogService.GetCatalogAsync();
                    _products.ReloadData(allProducts);

                    await NavigationService.PopAsync();
                });
        }

        private Task ViewBasket()
        {
            return NavigationService.NavigateToAsync ("Basket");
        }
    }
}