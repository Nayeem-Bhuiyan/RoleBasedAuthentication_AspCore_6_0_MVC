using NayeemApplication.Data.Entity.ApplicationUsersEntity;
using NayeemApplication.Data.Entity.MasterDataEntity;
using System.ComponentModel.DataAnnotations;
namespace NayeemApplication.Areas.Auth.Models.AccountViewModels
{
    public class RegisterViewModel
    {
       
        public string Name { get; set; }//User Name
        [Required]
        [RegularExpression(@"^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}$", ErrorMessage = "Invalid email")]
        public string Email { get; set; }
        public string RoleId { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string ConfirmPassword { get; set; }
        public bool RememberMe { get; set; }
        public string CountryId { get; set; } //fk
        //Customer Table
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        public string Website { get; set; }
        public string Company { get; set; }

        public IEnumerable<ApplicationRoleViewModel> userRoles { get; set; }
        public IEnumerable<Country> countries { get; set; }
        public IEnumerable<AspNetUsersViewModel> aspNetUsers { get; set; }
    }
}
