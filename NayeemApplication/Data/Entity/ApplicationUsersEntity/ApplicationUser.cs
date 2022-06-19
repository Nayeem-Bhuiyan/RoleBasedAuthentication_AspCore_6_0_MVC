using Microsoft.AspNetCore.Identity;
using NayeemApplication.Data.Entity.MasterDataEntity;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NayeemApplication.Data.Entity.ApplicationUsersEntity
{
    public class ApplicationUser : IdentityUser
    {

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
        [ForeignKey("City")]
        public int userCity { get; set; }
        public virtual City City { get; set; }

        public bool isActive { get; set; }
    }
}
