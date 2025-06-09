using System.ComponentModel.DataAnnotations;

namespace GP.PL.VIewModel
{
	public class ResetPasswordViewModel
	{
		[Required(ErrorMessage = "Password is required")]
		[MinLength(5, ErrorMessage = "Minimum Password Length is 5")]
		[DataType(DataType.Password)]
		public string NewPassword { get; set; }

		[Required(ErrorMessage = "Confirm Password is required")]
		[Compare(nameof(NewPassword), ErrorMessage = "Confirm Password does not match Password")]
		[DataType(DataType.Password)]
		public string ConfirmPassword { get; set; }
	}
}
