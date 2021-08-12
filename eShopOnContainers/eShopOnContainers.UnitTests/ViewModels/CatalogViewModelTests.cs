using eShopOnContainers.Core.Models.Catalog;
using eShopOnContainers.Core.Services.Catalog;
using eShopOnContainers.Core.Services.Settings;
using eShopOnContainers.Core.ViewModels;
using eShopOnContainers.Core.ViewModels.Base;
using eShopOnContainers.UnitTests.Mocks;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace eShopOnContainers.UnitTests
{
    public class CatalogViewModelTests
    {
        public CatalogViewModelTests()
        {
            ViewModelLocator.UpdateDependencies(true);
        }

        [Fact]
        public void AddCatalogItemCommandIsNotNullTest()
        {
            Xamarin.Forms.DependencyService.RegisterSingleton<ISettingsService>(new MockSettingsService());
            Xamarin.Forms.DependencyService.RegisterSingleton<ICatalogService>(new CatalogMockService());
            var catalogViewModel = new CatalogViewModel();

            Assert.NotNull(catalogViewModel.AddCatalogItemCommand);
        }

        [Fact]
        public void FilterCommandIsNotNullTest()
        {
            Xamarin.Forms.DependencyService.RegisterSingleton<ISettingsService>(new MockSettingsService());
            Xamarin.Forms.DependencyService.RegisterSingleton<ICatalogService>(new CatalogMockService());
            var catalogViewModel = new CatalogViewModel();

            Assert.NotNull(catalogViewModel.FilterCommand);
        }

        [Fact]
        public void ClearFilterCommandIsNotNullTest()
        {
            Xamarin.Forms.DependencyService.RegisterSingleton<ISettingsService>(new MockSettingsService());
            Xamarin.Forms.DependencyService.RegisterSingleton<ICatalogService>(new CatalogMockService());
            var catalogViewModel = new CatalogViewModel();

            Assert.NotNull(catalogViewModel.ClearFilterCommand);
        }

        [Fact]
        public void ProductsPropertyIsNullWhenViewModelInstantiatedTest()
        {
            Xamarin.Forms.DependencyService.RegisterSingleton<ISettingsService>(new MockSettingsService());
            Xamarin.Forms.DependencyService.RegisterSingleton<ICatalogService>(new CatalogMockService());
            var catalogViewModel = new CatalogViewModel();

            Assert.Null(catalogViewModel.Products);
        }

        [Fact]
        public void BrandsPropertyuIsNullWhenViewModelInstantiatedTest()
        {
            Xamarin.Forms.DependencyService.RegisterSingleton<ISettingsService>(new MockSettingsService());
            Xamarin.Forms.DependencyService.RegisterSingleton<ICatalogService>(new CatalogMockService());
            var catalogViewModel = new CatalogViewModel();

            Assert.Null(catalogViewModel.Brands);
        }

        [Fact]
        public void BrandPropertyIsNullWhenViewModelInstantiatedTest()
        {
            Xamarin.Forms.DependencyService.RegisterSingleton<ISettingsService>(new MockSettingsService());
            Xamarin.Forms.DependencyService.RegisterSingleton<ICatalogService>(new CatalogMockService());
            var catalogViewModel = new CatalogViewModel();

            Assert.Null(catalogViewModel.Brand);
        }

        [Fact]
        public void TypesPropertyIsNullWhenViewModelInstantiatedTest()
        {
            Xamarin.Forms.DependencyService.RegisterSingleton<ISettingsService>(new MockSettingsService());
            Xamarin.Forms.DependencyService.RegisterSingleton<ICatalogService>(new CatalogMockService());
            var catalogViewModel = new CatalogViewModel();

            Assert.Null(catalogViewModel.Types);
        }

        [Fact]
        public void TypePropertyIsNullWhenViewModelInstantiatedTest()
        {
            Xamarin.Forms.DependencyService.RegisterSingleton<ISettingsService>(new MockSettingsService());
            Xamarin.Forms.DependencyService.RegisterSingleton<ICatalogService>(new CatalogMockService());
            var catalogViewModel = new CatalogViewModel();

            Assert.Null(catalogViewModel.Type);
        }

        [Fact]
        public void IsFilterPropertyIsFalseWhenViewModelInstantiatedTest()
        {
            Xamarin.Forms.DependencyService.RegisterSingleton<ISettingsService>(new MockSettingsService());
            Xamarin.Forms.DependencyService.RegisterSingleton<ICatalogService>(new CatalogMockService());
            var catalogViewModel = new CatalogViewModel();

            Assert.False(catalogViewModel.IsFilter);
        }

        [Fact]
        public async Task ProductsPropertyIsNotNullAfterViewModelInitializationTest()
        {
            Xamarin.Forms.DependencyService.RegisterSingleton<ISettingsService>(new MockSettingsService());
            Xamarin.Forms.DependencyService.RegisterSingleton<ICatalogService>(new CatalogMockService());
            var catalogViewModel = new CatalogViewModel();

            await catalogViewModel.InitializeAsync(null);

            Assert.NotNull(catalogViewModel.Products);
        }

        [Fact]
        public async Task BrandsPropertyIsNotNullAfterViewModelInitializationTest()
        {
            Xamarin.Forms.DependencyService.RegisterSingleton<ISettingsService>(new MockSettingsService());
            Xamarin.Forms.DependencyService.RegisterSingleton<ICatalogService>(new CatalogMockService());
            var catalogViewModel = new CatalogViewModel();

            await catalogViewModel.InitializeAsync(null);

            Assert.NotNull(catalogViewModel.Brands);
        }

        [Fact]
        public async Task TypesPropertyIsNotNullAfterViewModelInitializationTest()
        {
            Xamarin.Forms.DependencyService.RegisterSingleton<ISettingsService>(new MockSettingsService());
            Xamarin.Forms.DependencyService.RegisterSingleton<ICatalogService>(new CatalogMockService());
            var catalogViewModel = new CatalogViewModel();

            await catalogViewModel.InitializeAsync(null);

            Assert.NotNull(catalogViewModel.Types);
        }

        [Fact]
        public async Task SettingProductsPropertyShouldRaisePropertyChanged()
        {
            bool invoked = false;

            Xamarin.Forms.DependencyService.RegisterSingleton<ISettingsService>(new MockSettingsService());
            Xamarin.Forms.DependencyService.RegisterSingleton<ICatalogService>(new CatalogMockService());
            var catalogViewModel = new CatalogViewModel();

            catalogViewModel.PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName.Equals("Products"))
                    invoked = true;
            };
            await catalogViewModel.InitializeAsync(null);

            Assert.True(invoked);
        }

        [Fact]
        public async Task SettingBrandsPropertyShouldRaisePropertyChanged()
        {
            bool invoked = false;

            Xamarin.Forms.DependencyService.RegisterSingleton<ISettingsService>(new MockSettingsService());
            Xamarin.Forms.DependencyService.RegisterSingleton<ICatalogService>(new CatalogMockService());
            var catalogViewModel = new CatalogViewModel();

            catalogViewModel.PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName.Equals("Brands"))
                    invoked = true;
            };
            await catalogViewModel.InitializeAsync(null);

            Assert.True(invoked);
        }

        [Fact]
        public async Task SettingTypesPropertyShouldRaisePropertyChanged()
        {
            bool invoked = false;

            Xamarin.Forms.DependencyService.RegisterSingleton<ISettingsService>(new MockSettingsService());
            Xamarin.Forms.DependencyService.RegisterSingleton<ICatalogService>(new CatalogMockService());
            var catalogViewModel = new CatalogViewModel();

            catalogViewModel.PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName.Equals("Types"))
                    invoked = true;
            };
            await catalogViewModel.InitializeAsync(null);

            Assert.True(invoked);
        }

        [Fact]
        public async Task ClearFilterCommandResetsPropertiesTest()
        {
            Xamarin.Forms.DependencyService.RegisterSingleton<ISettingsService>(new MockSettingsService());
            Xamarin.Forms.DependencyService.RegisterSingleton<ICatalogService>(new CatalogMockService());
            var catalogViewModel = new CatalogViewModel();

            await catalogViewModel.InitializeAsync(null);
            catalogViewModel.ClearFilterCommand.Execute(null);

            Assert.Null(catalogViewModel.Brand);
            Assert.Null(catalogViewModel.Type);
            Assert.NotNull(catalogViewModel.Products);
        }
    }
}
