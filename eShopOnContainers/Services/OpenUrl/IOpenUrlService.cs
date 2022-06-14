using System.Threading.Tasks;

namespace eShopOnContainers.Services.OpenUrl
{
    public interface IOpenUrlService
    {
        Task OpenUrl(string url);
    }
}
