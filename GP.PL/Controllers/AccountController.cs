using GP.DAL.Data.Models;
using GP.DAL.Models;
using GP.PL.Helper;
using GP.PL.VIewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;


namespace GP.PL.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public IActionResult SignIn()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> SignIn(SignInViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user is not null)
                {
                    var flag = await _userManager.CheckPasswordAsync(user, model.Password);
                    if (flag)
                    {
                        var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, false);
                        if (result.Succeeded)
                            return RedirectToAction(nameof(PatientController.Index), "Patient");
                    }
                }
                ModelState.AddModelError(string.Empty, "Invalid Login");
            }

            return View(model);
        }

        public IActionResult SignUp()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> SignUp(SingUpViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(model.UserName);
                if (user == null)
                {
                    user = new ApplicationUser
                    {
                        UserName = model.UserName,
                        Fname = model.FName,
                        Lname = model.LName,
                        Email = model.Email,
                        IsAgree = model.IsAgree,
                    };

                    var result = await _userManager.CreateAsync(user, model.Password);
                    if (result.Succeeded)
                        return RedirectToAction(nameof(AccountController.SignIn));

                    foreach (var error in result.Errors)
                        ModelState.AddModelError(string.Empty, error.Description);

                }
                ModelState.AddModelError(string.Empty, "Email is Already Exists");
            }

            return View(model);
        }

        public new async Task<IActionResult> SignOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(SignIn));
        }



        public IActionResult ForgetPassowrd()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> SendResetPasswordUrl(ForgetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);

                if (user is not null)
                {
                    var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                    var resetPasswordUrl = Url.Action("ResetPassword", "Account", new { email = model.Email, token = token }, Request.Scheme);
                    var email = new Email
                    {
                        Subject = "Reset Password",
                        Recipients = model.Email,
                        Body = resetPasswordUrl
                    };
                    EmailSetting.SendEmail(email);
                    return RedirectToAction(nameof(CheckYourInbox));
                }
                ModelState.AddModelError(string.Empty, "Invalid Email");

            }
            return View(model);
        }
        public IActionResult CheckYourInbox()
        {
            return View();
        }

        public IActionResult ResetPassword(string email, string token)
        {
            TempData["email"] = email;
            TempData["token"] = token;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                string email = TempData["email"] as string;
                string token = TempData["token"] as string;

                var user = await _userManager.FindByEmailAsync(email);
                var result = await _userManager.ResetPasswordAsync(user, token, model.NewPassword);
                if (result.Succeeded)
                    return RedirectToAction(nameof(SignIn));

                foreach (var error in result.Errors)
                    ModelState.AddModelError(string.Empty, error.Description);
            }
            return View(model);
        }

        public async Task<IActionResult> Profile()
        {
            var user = await _userManager.GetUserAsync(User);
            var model = new UserViewModel
            {
                Fname = user.Fname,
                Lname = user.Lname,
                Email = user.Email,
                ProfilePhoto = user.ProfilePhoto,

            };
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateProfile(UserViewModel model)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (!ModelState.IsValid)
            {
                return View("Profile", currentUser);
            }


            currentUser.Fname = model.Fname;
            currentUser.Lname = model.Lname;
            currentUser.Email = model.Email;
           
            var result = await _userManager.UpdateAsync(currentUser);
            if (result.Succeeded)
            {
                return RedirectToAction("Profile", currentUser);
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return View("Profile", currentUser);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        
        public async Task<IActionResult> ChangePassword(UserViewModel model) 
        {
            var currentUser = await _userManager.GetUserAsync(User);

            if (!ModelState.IsValid)
            {
                
                return View("Profile", currentUser);
            }

            
            var result = await _userManager.ChangePasswordAsync(
                currentUser,
                model.ChangePassword.CurrentPassword, 
                model.ChangePassword.NewPassword
            );

            if (result.Succeeded)
            {
                return RedirectToAction("Profile", currentUser);
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            
            return View("Profile", currentUser);
        }
        [HttpPost]
         [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateProfileImage(UserViewModel model)
    {
        
            

            var user = await _userManager.GetUserAsync(User);

            //user.ProfilePhoto = DocumentSettings.UpdloadFile(model.Image, "images");
            if (model.Image != null)
            {
                // Delete the old image
                if (!string.IsNullOrEmpty(user.ProfilePhoto))
                {
                    DocumentSettings.DeleteFile(user.ProfilePhoto, "Images");
                }

                // Upload the new image and set the new image name
                user.ProfilePhoto = DocumentSettings.UpdloadFile(model.Image, "Images");
            }
           

            var result=   await _userManager.UpdateAsync(user);
             if(result.Succeeded)
                return RedirectToAction("Profile");
            
            return View(model);



        }

    }
}
