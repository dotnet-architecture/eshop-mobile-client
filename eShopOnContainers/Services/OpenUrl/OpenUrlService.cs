using System;
using System.Threading.Tasks;

using Microsoft.Maui;

namespace eShopOnContainers.Services.OpenUrl
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
