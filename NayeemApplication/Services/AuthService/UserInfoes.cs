using NayeemApplication.Services.AuthService.Interfaces;
using NayeemApplication.Data;
using Microsoft.EntityFrameworkCore;
using NayeemApplication.Data.Entity.ApplicationUsersEntity;
using NayeemApplication.Areas.Auth.Models.AccountViewModels;
using System.Data;
namespace NayeemApplication.Services.AuthService
{
    public class UserInfoes: IUserInfoes
    {
        private readonly AppDbContext _context;
        public UserInfoes(AppDbContext context)
        {
            _context = context;
        }

        public async Task<string> GetUserRoleByUserName(string userName)
        {
            var name = "";
            var user = await _context.Users.Where(x => x.UserName == userName).FirstOrDefaultAsync();
            var userRole = await _context.UserRoles.Where(x => x.UserId == user.Id).FirstOrDefaultAsync();
            if (userRole != null)
            {
                var role = await _context.Roles.Where(x => x.Id == userRole.RoleId).FirstOrDefaultAsync();
                name = role?.Name;
            }
            else { name = "no roles assingn"; }
            return name;
        }

        public async Task<ApplicationUser> GetUserInfoByUserPhoneNumber(string phoneNumber)
        {
            return await _context.Users.Where(x => x.PhoneNumber == phoneNumber).AsNoTracking().FirstOrDefaultAsync();
        }


        public async Task<IEnumerable<ApplicationUser>> GetUsers()
        {
            return await _context.Users.AsNoTracking().ToListAsync();
        }


        public async Task<bool> DeleteRoleById(string Id)
        {
            _context.Roles.Remove(_context.Roles.Where(x => x.Id == Id).First());
            return 1 == await _context.SaveChangesAsync();
        }

       

        public async Task<string> GetUserRoleByByUserId(string userId)
        {
            var name = "";
            var user = await _context.Users.Where(x => x.Id == userId).FirstOrDefaultAsync();
            var userRole = await _context.UserRoles.Where(x => x.UserId == user.Id).FirstOrDefaultAsync();
            if (userRole != null)
            {
                var role = await _context.Roles.Where(x => x.Id == userRole.RoleId).FirstOrDefaultAsync();
                name = role?.Name;
            }
            else { name = "no roles assingn"; }
            return name;
        }

        public async Task<string> CheckUserName(string uname)
        {
            var user= await _context.Users.Where(x => x.UserName == uname).Select(x => x.UserName).FirstOrDefaultAsync();
            if(user == null)
            {
                user = "Not Used";
            }
            return user;
        }
        public async Task<string> CheckEmail(string email)
        {
            var user = await _context.Users.Where(x => x.Email == email).Select(x => x.Email).FirstOrDefaultAsync();
            if (user == null)
            {
                user = "Not Used";
            }
            return user;
        }


        public async Task<string> CheckPhone(string phoneNumber)
        {
            var user = await _context.Users.Where(x => x.PhoneNumber == phoneNumber).Select(x => x.PhoneNumber).FirstOrDefaultAsync();
            if (user == null)
            {
                user = "Not Used";
            }
            return user;
        }


        public async Task<string> DeleteUser(string id)
        {
            try
            {
                var user = await _context.Users.Where(s => s.Id == id).FirstOrDefaultAsync();
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
                return user.UserName;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
            
        }
        public async Task<string> UpdateUserStatusByUserIdAndStatus(string id, int status)
        {
            try
            {
                var user = await _context.Users.Where(x => x.Id == id).FirstOrDefaultAsync();
                user.isActive = status==1?true:false;
                 _context.Users.Update(user);
                await _context.SaveChangesAsync();
                return "success";
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        



    }
}
