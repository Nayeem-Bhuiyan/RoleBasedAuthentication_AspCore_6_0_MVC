using NayeemApplication.Data.Entity.MasterDataEntity;

namespace NayeemApplication.Services.CityService.Interface
{
    public interface ICityService
    {
        Task<IEnumerable<City>> GetCitybyCountryIdsAsync(int CountryId);
    }
}
