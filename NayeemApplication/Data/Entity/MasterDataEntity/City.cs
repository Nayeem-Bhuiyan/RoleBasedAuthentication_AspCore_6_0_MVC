using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NayeemApplication.Data.Entity.MasterDataEntity
{
    public class City:Base
    {
        [Display(Name = "First Name")]
        [StringLength(100, ErrorMessage = "")]
        [Required]
        [RegularExpression("^[a-zA-Z ]*$")]
        public string cityName { get; set; }
        [RegularExpression("([0-9]+)")]

        [ForeignKey("Country")]
        public int CountryId { get; set; }
        public virtual Country Country { get; set; }
    }
}
