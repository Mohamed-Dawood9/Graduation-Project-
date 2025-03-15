using Microsoft.AspNetCore.Mvc;

namespace GP.PL.Controllers
{
	public class AccountController : Controller
	{
		public IActionResult SignUp()
		{
			return View();
		}
	}
}
