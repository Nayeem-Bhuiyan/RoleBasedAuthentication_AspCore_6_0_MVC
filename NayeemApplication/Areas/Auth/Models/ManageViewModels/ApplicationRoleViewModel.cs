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
        public string Email { get; set; }
        public bool? isActive { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public DateTime? createdAt { get; set; }
        public string mobileNo { get; set; }
        public string email { get; set; }

        public string roleId { get; set; }
        public string roleName { get; set; }
        [DataType(DataType.ImageUrl)]
        [Display(Name = "Image ")]
        [StringLength(300, ErrorMessage = "")]
        [Required]
        public string userImg { get; set; }
        [DataType(DataType.Url)]
        [Display(Name = "CV Upload")]
        [StringLength(300, ErrorMessage = "")]
        [Required]
        public string userCV { get; set; }

        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [Display(Name = "Date Of Birth")]
        public DateTime dob { get; set; }
        [RegularExpression("([0-9]+)")]
        public int userCityId { get; set; }
        public string userCityName { get; set; }

        public int userCountryId { get; set; }
        public string userCountryName { get; set; }



    }
}
