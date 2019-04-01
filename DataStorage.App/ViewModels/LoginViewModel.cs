using System.ComponentModel.DataAnnotations;

namespace DataStorage.App.ViewModels
{
    public class LoginViewModel
    {
        [Required]
        [RegularExpression("[a-z0-9._%+-]+@[a-z0-9.-]+\\.[a-z]{2,3}$", ErrorMessage = "E - mail should look like: example@gmail.com")]
        public string Email { get; set; }
        [Required]
        [RegularExpression("(?=.*\\d)(?=.*[a-z])(?=.*[A-Z]).{8,}", ErrorMessage = "Password must be at least 8 characters long and contain at least one uppercase, lowercase letter and one number.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public bool rememberMe { get; set; }
    }
}


//[RegularExpression("[a-z0-9._%+-]+@[a-z0-9.-]+\\.[a-z]{2,3}$", ErrorMessage = "E - mail should look like: example@gmail.com")]

