using NayeemApplication.Data.Entity.MasterDataEntity;

namespace NayeemApplication.Services.CountryService.Interface
{
    public interface ICountryService
    {
        Task<IEnumerable<Country>> GetAllCountrysAsync();
    }
}
