using System.ComponentModel.DataAnnotations;

using StudentSystem.Common.Constants;

namespace StudentSystem.Clients.Mvc.ViewModels.Account
{
    public class RegisterViewModel
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

        [DataType(DataType.Password)]
        [Display(Name = ClientMessage.ConfirmPassword)]
        [Compare(nameof(Password), ErrorMessage = ClientMessage.PasswordDoesNotMatch)]
        public string ConfirmPassword { get; set; }
    }
}