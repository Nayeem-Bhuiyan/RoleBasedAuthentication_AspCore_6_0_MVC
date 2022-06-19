using NayeemApplication.Data.Entity.ApplicationUsersEntity;
using NayeemApplication.Areas.Auth.Models.AccountViewModels;

namespace NayeemApplication.Services.AuthService.Interfaces
{
    public interface IUserInfoes
    {

        Task<string> GetUserRoleByUserName(string userName);
        Task<ApplicationUser> GetUserInfoByUserPhoneNumber(string phoneNumber);
        Task<bool> DeleteRoleById(string Id);
        Task<string> CheckUserName(string uname);
        Task<string> CheckEmail(string email);
        Task<string> CheckPhone(string phoneNumber);
        Task<string> DeleteUser(string id);
        Task<string> UpdateUserStatusByUserIdAndStatus(string id, int status);
        Task<IEnumerable<AspNetUsersViewModel>> GetUserInfoList();
        Task<string> GetUserRoleByByUserId(string userId);
        Task<IEnumerable<ApplicationUser>> GetUsers();

    }
}
