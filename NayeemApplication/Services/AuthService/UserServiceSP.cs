using Microsoft.Data.SqlClient;
using NayeemApplication.Data.Entity.ApplicationUsersEntity;
using NayeemApplication.Services.AuthService.Interfaces;
using System.Data;
namespace NayeemApplication.Services.AuthService
{
    public class UserServiceSP: IUserServiceSP
    {
        private readonly string _connectionString;

        public UserServiceSP(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("AppDbConnection");
        }


        public async Task<ApplicationUser> GetUserInfoByUser(string userName)
        {
            ApplicationUser applicationUser = new ApplicationUser();
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("GetUserInfoByUser", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@UserName", userName);
                    await sql.OpenAsync();
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            applicationUser=MapToApplicationUser(reader);
                        }
                    }


                }

                return applicationUser;
            }


        }

        public async Task<ApplicationUser> GetUserInfoByEmailAsync(string email)
        {
            ApplicationUser applicationUser = new ApplicationUser();
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("Sp_GetUserInfoByEmail", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Email", email);
                    await sql.OpenAsync();
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            applicationUser=MapToApplicationUser(reader);
                        }
                    }


                }

                return applicationUser;
            }


        }

        private ApplicationUser MapToApplicationUser(SqlDataReader reader)
        {
            return new ApplicationUser()
            {
                Id =reader["Id"].ToString(),
                userImg=reader["userImg"].ToString(),
                userCV=reader["userCV"].ToString(),
                dob=Convert.ToDateTime(reader["dob"].ToString()),
                userCity=Convert.ToInt32(reader["userCity"].ToString()),
                isActive=Convert.ToBoolean(reader["isActive"].ToString()),
                UserName=reader["UserName"].ToString(),
                NormalizedUserName=reader["NormalizedUserName"].ToString(),
                Email=reader["Email"].ToString(),
                NormalizedEmail=reader["NormalizedEmail"].ToString(),
                PhoneNumber=reader["PhoneNumber"].ToString(),
                PasswordHash=reader["PasswordHash"].ToString(),
                EmailConfirmed=Convert.ToBoolean(reader["EmailConfirmed"]),
                SecurityStamp=reader["SecurityStamp"].ToString(),
                ConcurrencyStamp=reader["ConcurrencyStamp"].ToString(),
                //PhoneNumberConfirmed=Convert.ToBoolean(reader["PhoneNumberConfirm"]),
                TwoFactorEnabled=Convert.ToBoolean(reader["TwoFactorEnabled"]),
                //LockoutEnd=Convert.ToDateTime(reader["LockoutEnd"]),
                LockoutEnabled=Convert.ToBoolean(reader["LockoutEnabled"]),
                AccessFailedCount=Convert.ToInt32(reader["AccessFailedCount"]),

            };
        }

    }
}
