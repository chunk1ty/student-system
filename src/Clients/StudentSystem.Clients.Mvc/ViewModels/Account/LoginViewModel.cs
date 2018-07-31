using System.ComponentModel.DataAnnotations;

using StudentSystem.Common.Constants;

namespace StudentSystem.Clients.Mvc.ViewModels.Account
{
    public class LoginViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = ClientMessage.Email)]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = ClientMessage.Password)]
        [StringLength(100, ErrorMessage = ClientMessage.MaxAndMinLength, MinimumLength = 6)]
        public string Password { get; set; }

        [Display(Name = ClientMessage.RememberMe)]
        public bool RememberMe { get; set; }
    }
}