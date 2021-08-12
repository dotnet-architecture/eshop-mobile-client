using System.Threading.Tasks;

namespace eShopOnContainers.Core.Services.OpenUrl
{
    public interface IOpenUrlService
    {
        Task OpenUrl(string url);
    }
}
