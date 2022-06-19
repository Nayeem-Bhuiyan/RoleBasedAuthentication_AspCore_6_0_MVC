namespace NayeemApplication.Areas.Auth.Models.AccountViewModels
{
    public class UserListViewModel
    {
        public IEnumerable<AspNetUsersViewModel> aspNetUsersViewModels { get; set; }
        public IEnumerable<ApplicationRoleViewModel> userRoles { get; set; }

    }
}
