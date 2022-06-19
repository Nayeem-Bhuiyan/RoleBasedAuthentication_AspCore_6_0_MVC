using Microsoft.AspNetCore.Mvc.Rendering;
namespace NayeemApplication.Areas.Auth.Models.ManageViewModels
{
    public class ConfigureTwoFactorViewModel
    {
        public string SelectedProvider { get; set; }
        public ICollection<SelectListItem> Providers { get; set; }
    }
}
