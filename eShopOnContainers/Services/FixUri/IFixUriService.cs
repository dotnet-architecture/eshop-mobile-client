using System.Collections.Generic;
using eShopOnContainers.Models.Basket;
using eShopOnContainers.Models.Catalog;
using eShopOnContainers.Models.Marketing;

namespace eShopOnContainers.Services.FixUri
{
    public interface IFixUriService
    {
        void FixCatalogItemPictureUri(IEnumerable<CatalogItem> catalogItems);
        void FixBasketItemPictureUri(IEnumerable<BasketItem> basketItems);
        void FixCampaignItemPictureUri(IEnumerable<CampaignItem> campaignItems);
    }
}
