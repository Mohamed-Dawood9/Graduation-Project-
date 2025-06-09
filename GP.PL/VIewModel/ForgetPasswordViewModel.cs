using System.ComponentModel.DataAnnotations;

namespace GP.PL.VIewModel
{
	public class ForgetPasswordViewModel
	{

		[Required(ErrorMessage = "Email is required")]
		[EmailAddress(ErrorMessage = "Invalid Email")]
		public string Email { get; set; }	
	}
}
