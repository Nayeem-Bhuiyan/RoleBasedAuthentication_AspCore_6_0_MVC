using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using NayeemApplication.Data.Entity;
using NayeemApplication.Data.Entity.ApplicationUsersEntity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using NayeemApplication.Data.Entity.MasterDataEntity;

namespace NayeemApplication.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        internal object tblName;

        public AppDbContext(DbContextOptions<AppDbContext> options, IHttpContextAccessor _httpContextAccessor) : base(options)
        {
            this._httpContextAccessor = _httpContextAccessor;
        }

        #region MasterData
        public DbSet<Country> Country { get; set; }
        public DbSet<City> City { get; set; }
        #endregion

 

    }
}
