using System;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace eShopOnContainers.Core.Services.OpenUrl
{
    public class OpenUrlService : IOpenUrlService
    {
        public async Task OpenUrl(string url)
        {
            if (await Launcher.CanOpenAsync(url))
            {
                await Launcher.OpenAsync (url);
            }
        }
    }
}
