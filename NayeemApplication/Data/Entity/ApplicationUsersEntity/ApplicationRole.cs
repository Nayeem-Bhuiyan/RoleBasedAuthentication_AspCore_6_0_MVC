using Microsoft.AspNetCore.Identity;

namespace NayeemApplication.Data.Entity.ApplicationUsersEntity
{
    public class ApplicationRole : IdentityRole
    {
        public ApplicationRole() : base() { }
        public ApplicationRole(string roleName) : base(roleName)
        {
            
        }
    }
}
