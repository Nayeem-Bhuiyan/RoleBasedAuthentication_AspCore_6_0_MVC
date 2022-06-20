using NayeemApplication.Data.Entity.MasterDataEntity;

namespace NayeemApplication.Areas.Auth.Models.AccountViewModels
{
    public class UserListViewModel
    {
        public IEnumerable<AspNetUsersViewModel> aspNetUsersViewModels { get; set; }
        public IEnumerable<ApplicationRoleViewModel> userRoles { get; set; }
        public IEnumerable<Country> Countries { get; set; }
        public IEnumerable<City> Cities { get; set; }

    }
}
