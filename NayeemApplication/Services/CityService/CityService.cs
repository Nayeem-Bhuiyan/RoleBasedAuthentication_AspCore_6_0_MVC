using NayeemApplication.Data.Entity.MasterDataEntity;
using Microsoft.Data.SqlClient;
using System.Data;
namespace NayeemApplication.Services.CityService
{
    public class CityService
    {
        private readonly string _connectionString;

        public CityService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("AppDbConnection");
        }


        public async Task<IEnumerable<City>> GetCitybyCountryIdsAsync(int CountryId)
        {
            List<City> listCity = new List<City>();
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("Sp_GetCityListByCountryId", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@CountryId", CountryId);
                    await sql.OpenAsync();
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            listCity.Add(MapToALLCity(reader));
                        }
                    }


                }

                return listCity;
            }


        }

        private City MapToALLCity(SqlDataReader reader)
        {
            return new City()
            {
                Id =Convert.ToInt32(reader["Id"].ToString()),
                cityName = reader["cityName"].ToString(),
            };
        }
    }
}
