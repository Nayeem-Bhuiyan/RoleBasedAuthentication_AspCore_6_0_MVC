using NayeemApplication.Data.Entity.MasterDataEntity;
using Microsoft.Data.SqlClient;
using System.Data;
using NayeemApplication.Services.CountryService.Interface;

namespace NayeemApplication.Services.CountryService
{
    public class CountryService: ICountryService
    {
        private readonly string _connectionString;

        public CountryService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("AppDbConnection");
        }


        public async Task<IEnumerable<Country>> GetAllCountrysAsync()
        {
            List<Country> listCountry = new List<Country>();
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("Sp_AllCountries", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    await sql.OpenAsync();
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            listCountry.Add(MapToALLCountry(reader));
                        }
                    }


                }

                return listCountry;
            }


        }

        private Country MapToALLCountry(SqlDataReader reader)
        {
            return new Country()
            {
                Id =Convert.ToInt32(reader["Id"].ToString()),
                CountryName = reader["CountryName"].ToString(),
            };
        }


    }
}
