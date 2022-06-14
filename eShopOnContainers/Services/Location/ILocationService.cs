using System.Threading.Tasks;

namespace eShopOnContainers.Services.Location
{    
    public interface ILocationService
    {
        Task UpdateUserLocation(eShopOnContainers.Models.Location.Location newLocReq, string token);
    }
}