using System.ComponentModel.DataAnnotations;

namespace GP.PL.VIewModel
{
    public class UserViewModel
    {
        [Display(Name ="First Name")]
        public string Fname { get; set; }
        [Display(Name = "Last Name")]

        public string Lname { get; set; }
        [Display(Name ="Profile Photo")]
        public string Email { get; set; }
        public string? ProfilePhoto { get; set; }

        public IFormFile? Image { get; set; }
        public ChangePasswordViewModel? ChangePassword { get; set; } 
    }
   

    public class ChangePasswordViewModel
    {
        [DataType(DataType.Password)]
        [Required]
        public string CurrentPassword { get; set; }

        [DataType(DataType.Password)]
        [Required]
        [StringLength(100, MinimumLength = 6)]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Compare("NewPassword")]
        public string ConfirmPassword { get; set; }
    }
}
