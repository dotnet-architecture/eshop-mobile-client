using System;
using eShopOnContainers.Services.Basket;
using eShopOnContainers.Services.Catalog;
using eShopOnContainers.Services.Marketing;
using eShopOnContainers.Services.Order;
using eShopOnContainers.Services.User;

namespace eShopOnContainers.Services.AppEnvironment
{
    public interface IAppEnvironmentService
    {
        IBasketService BasketService { get; }
        ICampaignService CampaignService { get; }
        ICatalogService CatalogService { get; }
        IOrderService OrderService { get; }
        IUserService UserService { get; }

        void UpdateDependencies(bool useMockServices);
    }
}

