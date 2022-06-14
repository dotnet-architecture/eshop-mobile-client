using eShopOnContainers.Extensions;
using eShopOnContainers.Models.Catalog;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Maui;

namespace eShopOnContainers.Services.Catalog
{
    public class CatalogMockService : ICatalogService
    {
        private IEnumerable<CatalogBrand> MockCatalogBrand =
            new []
            {
                new CatalogBrand { Id = 1, Brand = "Azure" },
                new CatalogBrand { Id = 2, Brand = "Visual Studio" }
            };

        private IEnumerable<CatalogType> MockCatalogType =
            new []
            {
                new CatalogType { Id = 1, Type = "Mug" },
                new CatalogType { Id = 2, Type = "T-Shirt" }
            };

        private IEnumerable<CatalogItem> MockCatalog =
            new []
            {
                new CatalogItem { Id = Common.Common.MockCatalogItemId01, PictureUri = "fake_product_01.png", Name = ".NET Bot Blue Sweatshirt (M)", Price = 19.50M, CatalogBrandId = 2, CatalogBrand = "Visual Studio", CatalogTypeId = 2, CatalogType = "T-Shirt" },
                new CatalogItem { Id = Common.Common.MockCatalogItemId02, PictureUri = "fake_product_02.png", Name = ".NET Bot Purple Sweatshirt (M)", Price = 19.50M, CatalogBrandId = 2, CatalogBrand = "Visual Studio", CatalogTypeId = 2, CatalogType = "T-Shirt" },
                new CatalogItem { Id = Common.Common.MockCatalogItemId03, PictureUri = "fake_product_03.png", Name = ".NET Bot Black Sweatshirt (M)", Price = 19.95M, CatalogBrandId = 2, CatalogBrand = "Visual Studio", CatalogTypeId = 2, CatalogType = "T-Shirt" },
                new CatalogItem { Id = Common.Common.MockCatalogItemId04, PictureUri = "fake_product_04.png", Name = ".NET Black Cupt", Price = 17.00M, CatalogBrandId = 2, CatalogBrand = "Visual Studio", CatalogTypeId = 1, CatalogType = "Mug" },
                new CatalogItem { Id = Common.Common.MockCatalogItemId05, PictureUri = "fake_product_05.png", Name = "Azure Black Sweatshirt (M)", Price = 19.50M, CatalogBrandId = 1, CatalogBrand = "Azure", CatalogTypeId = 2, CatalogType = "T-Shirt" }
            };

        public async Task<IEnumerable<CatalogItem>> GetCatalogAsync()
        {
            await Task.Delay(10);

            return MockCatalog;
        }

        public async Task<IEnumerable<CatalogItem>> FilterAsync(int catalogBrandId, int catalogTypeId)
        {
            await Task.Delay(10);

            return MockCatalog
                .Where(
                    c => c.CatalogBrandId == catalogBrandId &&
                    c.CatalogTypeId == catalogTypeId)
                .ToArray();
        }

        public async Task<IEnumerable<CatalogBrand>> GetCatalogBrandAsync()
        {
            await Task.Delay(10);

            return MockCatalogBrand;
        }

        public async Task<IEnumerable<CatalogType>> GetCatalogTypeAsync()
        {
            await Task.Delay(10);

            return MockCatalogType;
        }
    }
}