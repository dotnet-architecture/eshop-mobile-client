using eShopOnContainers.Models.Catalog;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace eShopOnContainers.Services.Catalog
{
    public interface ICatalogService
    {
        Task<IEnumerable<CatalogBrand>> GetCatalogBrandAsync();
        Task<IEnumerable<CatalogItem>> FilterAsync(int catalogBrandId, int catalogTypeId);
        Task<IEnumerable<CatalogType>> GetCatalogTypeAsync();
        Task<IEnumerable<CatalogItem>> GetCatalogAsync();
    }
}
