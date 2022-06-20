using NayeemApplication.Areas.Auth.Models.AccountViewModels;
using NayeemApplication.Data.Entity.ApplicationUsersEntity;

namespace NayeemApplication.Services.AuthService.Interfaces
{
    public interface IUserServiceSP
    {
        Task<ApplicationUser> GetUserInfoByUser(string userName);
        Task<ApplicationUser> GetUserInfoByEmailAsync(string email);
        Task<IEnumerable<AspNetUsersViewModel>> GetUserInfoList();
    }
}
