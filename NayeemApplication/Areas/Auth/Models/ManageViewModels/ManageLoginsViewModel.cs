using Microsoft.AspNetCore.Identity;
namespace NayeemApplication.Areas.Auth.Models.ManageViewModels
{
    public class ManageLoginsViewModel
    {
        public IList<UserLoginInfo> CurrentLogins { get; set; }
        //public IList<AuthenticationDescription> OtherLogins { get; set; }
    }
}
