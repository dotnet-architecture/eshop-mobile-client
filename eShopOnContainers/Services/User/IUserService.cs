using eShopOnContainers.Models.User;
using System.Threading.Tasks;

namespace eShopOnContainers.Services.User
{
    public interface IUserService
    {
        Task<UserInfo> GetUserInfoAsync(string authToken);
    }
}
