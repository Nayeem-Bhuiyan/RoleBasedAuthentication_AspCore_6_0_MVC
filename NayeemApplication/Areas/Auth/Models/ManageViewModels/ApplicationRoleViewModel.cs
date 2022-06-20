using System.ComponentModel.DataAnnotations;

namespace NayeemApplication.Areas.Auth.Models.AccountViewModels
{
    public class ApplicationRoleViewModel
    {
        public string RoleId { get; set; }
        public string PreRoleId { get; set; }
        public string[] roleIdList { get; set; }

        public string userId { get; set; }

        public string userName { get; set; }
        public string EuserName { get; set; }

        public string RoleName { get; set; }

        public string description { get; set; }



        public IEnumerable<ApplicationRoleViewModel> roleViewModels { get; set; }









        //extra



        public string aspnetId { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public bool? isActive { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string mobileNo { get; set; }
        public string email { get; set; }

        public string roleId { get; set; }
        public string roleName { get; set; }
        [Required]
        public IFormFile userImg { get; set; }
        [Required]
        public IFormFile userCV { get; set; }
        [RegularExpression("([0-9]+)")]
        public int userCityId { get; set; }
        public int userCountryId { get; set; }




    }
}
