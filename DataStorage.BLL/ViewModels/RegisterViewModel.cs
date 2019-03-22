using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace DataStorage.BLL.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "This field is required!")]
        [Display(Name = "Email")]
        [EmailAddress]
        //[RegularExpression("[a-z0-9._%+-]+@[a-z0-9.-]+\\.[a-z]{2,3}$", ErrorMessage = "E - mail should look like: example@gmail.com")]
        public string Email { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 8)]
        //[RegularExpression("(?=.*\\d)(?=.*[a-z])(?=.*[A-Z]).{8,}", ErrorMessage = "Password must be at least 8 characters long and contain at least one uppercase, lowercase letter and one number.")]
        [Required(ErrorMessage = "This field is required!")]
        public string Password { get; set; }

        [Required]
        //[RegularExpression("(?=.*\\d)(?=.*[a-z])(?=.*[A-Z]).{8,}", ErrorMessage = "Password must be at least 8 characters long and contain at least one uppercase, lowercase letter and one number.")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password does not match.")]
        public string ConfirmPassword { get; set; }
    }
}
