using System.ComponentModel.DataAnnotations;
namespace NayeemApplication.Areas.Auth.Models.AccountViewModels
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
