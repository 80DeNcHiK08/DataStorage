using System.ComponentModel.DataAnnotations;

namespace DataStorage.App.ViewModels
{
    public class DeleteUserViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [Compare("Email", ErrorMessage = "The Email and confirmation Email do not match.")]
        [EmailAddress]
        public string ConfirmEmail { get; set; }
    }
}