using System;
using System.Threading.Tasks;
using eShopOnContainers.Services.RequestProvider;
using eShopOnContainers.Models.Basket;
using eShopOnContainers.Services.FixUri;
using eShopOnContainers.Helpers;
using System.Collections.Generic;

namespace eShopOnContainers.Services.Basket
{
    public class BasketService : IBasketService
    {
        private readonly IRequestProvider _requestProvider;
        private readonly IFixUriService _fixUriService;

        private const string ApiUrlBase = "b/api/v1/basket";

        public IEnumerable<BasketItem> LocalBasketItems { get; set; }

        public BasketService(IRequestProvider requestProvider, IFixUriService fixUriService)
        {
            _requestProvider = requestProvider;
            _fixUriService = fixUriService;
        }

        public async Task<CustomerBasket> GetBasketAsync(string guidUser, string token)
        {
            var uri = UriHelper.CombineUri(GlobalSetting.Instance.GatewayShoppingEndpoint, $"{ApiUrlBase}/{guidUser}");

            CustomerBasket basket;

            try
            {
                basket = await _requestProvider.GetAsync<CustomerBasket>(uri, token);
            }
            catch (HttpRequestExceptionEx exception) when (exception.HttpCode == System.Net.HttpStatusCode.NotFound)
            {
                basket = null;
            }

            _fixUriService.FixBasketItemPictureUri(basket?.Items);
            return basket;
        }

        public async Task<CustomerBasket> UpdateBasketAsync(CustomerBasket customerBasket, string token)
        {
            var uri = UriHelper.CombineUri(GlobalSetting.Instance.GatewayShoppingEndpoint, ApiUrlBase);

            var result = await _requestProvider.PostAsync(uri, customerBasket, token);
            return result;
        }

        public async Task CheckoutAsync(BasketCheckout basketCheckout, string token)
        {
            var uri = UriHelper.CombineUri(GlobalSetting.Instance.GatewayShoppingEndpoint, $"{ApiUrlBase}/checkout");

            await _requestProvider.PostAsync(uri, basketCheckout, token);
        }

        public async Task ClearBasketAsync(string guidUser, string token)
        {
            var uri = UriHelper.CombineUri(GlobalSetting.Instance.GatewayShoppingEndpoint, $"{ApiUrlBase}/{guidUser}");

            await _requestProvider.DeleteAsync(uri, token);

            LocalBasketItems = null;
        }
    }
}