using eShopOnContainers.ViewModels.Base;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace eShopOnContainers.Services
{
    public interface INavigationService
    {
        Task InitializeAsync();

        Task NavigateToAsync (string route, IDictionary<string, object> routeParameters = null);

        Task PopAsync();
    }
}