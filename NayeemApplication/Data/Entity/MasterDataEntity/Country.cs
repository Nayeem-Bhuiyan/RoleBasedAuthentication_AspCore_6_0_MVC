using System.ComponentModel.DataAnnotations;

namespace NayeemApplication.Data.Entity.MasterDataEntity
{
    public class Country:Base
    {
        [Display(Name = "Country Name")]
        [StringLength(150, ErrorMessage = "")]
        [Required]
        [RegularExpression("^[a-zA-Z ]*$")]
        public string CountryName { get; set; }
    }
}
