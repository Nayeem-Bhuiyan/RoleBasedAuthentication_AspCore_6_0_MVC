using Microsoft.AspNetCore.Mvc;
using NayeemApplication.Services.CityService.Interface;

namespace NayeemApplication.Areas.MasterData.Controllers
{
    [Area("MasterData")]
    public class CityInfoController : Controller
    {
        public readonly ICityService _cityService;

        public CityInfoController(ICityService cityService)
        {
            _cityService =cityService;
        }

        public IActionResult Index()
        {
            return View();
        }


        public async Task<IActionResult> CityLoadByCountryId(int countryId)
        {
           return Json(await _cityService.GetCitybyCountryIdsAsync(countryId));
        }
    }
}
