using System.ComponentModel.DataAnnotations;

namespace NayeemApplication.Areas.Auth.Models.AccountViewModels
{
    public class AspNetUsersViewModel
    {
        public string aspnetId { get; set; }
        public string UserName { get; set; }
        public int UserId { get; set; }
        public string Email { get; set; }
        public int? UserTypeId { get; set; }
        public bool? isActive { get; set; }
        public string DivisionName { get; set; }
        public string DistrictName { get; set; }
        public string userTypeName { get; set; }
        public DateTime? joiningDate { get; set; }
        public DateTime? createdAt { get; set; }
        public string mobileNo { get; set; }
        public string email { get; set; }
        public int? status { get; set; }
        public int? photoId { get; set; }
        public string roleId { get; set; }
        public string roleName{ get; set; }
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
        public int userCity { get; set; }





    }
}
