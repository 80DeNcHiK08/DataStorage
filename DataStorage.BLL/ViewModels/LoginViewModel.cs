using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace DataStorage.BLL.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "This field is required!")]
        [Display(Name = "Email")]
        [RegularExpression("[a-z0-9._%+-]+@[a-z0-9.-]+\\.[a-z]{2,3}$", ErrorMessage = "E - mail should look like: example@gmail.com")]
        public string Email { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        [RegularExpression("(?=.*\\d)(?=.*[a-z])(?=.*[A-Z]).{8,}", ErrorMessage = "Password must be at least 8 characters long and contain at least one uppercase, lowercase letter and one number.")]
        [Required(ErrorMessage = "This field is required!")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }
}
