using eShopOnContainers.Models.Marketing;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace eShopOnContainers.Services.Marketing
{
    public interface ICampaignService
    {
        Task<IEnumerable<CampaignItem>> GetAllCampaignsAsync(string token);
        Task<CampaignItem> GetCampaignByIdAsync(int id, string token);
    }
}