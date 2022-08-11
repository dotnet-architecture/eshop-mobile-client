using eShopOnContainers.Models.User;

namespace eShopOnContainers.Services.User
{
    public interface IUserService
    {
        Task<UserInfo> GetUserInfoAsync(string authToken);
    }
}