using System.ComponentModel.DataAnnotations;

namespace GP.PL.VIewModel
{
    public class SingUpViewModel
    {

        public string FName { get; set; }
        public string LName { get; set; }
        public string UserName { get; set; }
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email")]

        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [MinLength(5, ErrorMessage = "Minimum Password Length is 5")]
        [DataType(DataType.Password)]

        public string Password { get; set; }

        [Required(ErrorMessage = "Confirm Password is required")]
        [Compare(nameof(Password), ErrorMessage = "Confirm Password does not match Password")]
        [DataType(DataType.Password)]

        public string ConfirmPassword { get; set; }

        public bool IsAgree { get; set; } 

    }
}
