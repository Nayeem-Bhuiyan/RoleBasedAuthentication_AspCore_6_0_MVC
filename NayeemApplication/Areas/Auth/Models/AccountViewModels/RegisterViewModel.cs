using NayeemApplication.Data.Entity.ApplicationUsersEntity;
using NayeemApplication.Data.Entity.MasterDataEntity;
using System.ComponentModel.DataAnnotations;
namespace NayeemApplication.Areas.Auth.Models.AccountViewModels
{
    public class RegisterViewModel
    {
       
        public string Name { get; set; }//User Name
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



        public IFormFile UploadUserPhoto { get; set; }
        public IFormFile UploadUserCV { get; set; }

        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [Display(Name = "Date Of Birth")]
        public DateTime dateOfBirth { get; set; }
        [RegularExpression("([0-9]+)")]
        public int userCity { get; set; }


        public IEnumerable<ApplicationRoleViewModel> userRoles { get; set; }
        public IEnumerable<Country> countries { get; set; }
        public IEnumerable<AspNetUsersViewModel> aspNetUsers { get; set; }
    }
}
