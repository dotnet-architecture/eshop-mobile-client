using eShopOnContainers.Models.Catalog;

namespace eShopOnContainers.Services.Catalog;

public interface ICatalogService
{
    Task<IEnumerable<CatalogBrand>> GetCatalogBrandAsync();
    Task<IEnumerable<CatalogItem>> FilterAsync(int catalogBrandId, int catalogTypeId);
    Task<IEnumerable<CatalogType>> GetCatalogTypeAsync();
    Task<IEnumerable<CatalogItem>> GetCatalogAsync();
}