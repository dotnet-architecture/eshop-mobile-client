using eShopOnContainers.Models.Basket;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace eShopOnContainers.Services.Basket
{
    public interface IBasketService
    {
        IEnumerable<BasketItem> LocalBasketItems { get; set; }
        Task<CustomerBasket> GetBasketAsync(string guidUser, string token);
        Task<CustomerBasket> UpdateBasketAsync(CustomerBasket customerBasket, string token);
        Task CheckoutAsync(BasketCheckout basketCheckout, string token);
        Task ClearBasketAsync(string guidUser, string token);
    }
}