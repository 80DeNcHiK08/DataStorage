using System.ComponentModel.DataAnnotations;

namespace DataStorage.App.ViewModels
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [RegularExpression("[a-z0-9._%+-]+@[a-z0-9.-]+\\.[a-z]{2,3}$", ErrorMessage = "E-mail should look like: example@gmail.com")]
        [EmailAddress]
        public string Email { get; set; }
    }
}