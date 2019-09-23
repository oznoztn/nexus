using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nexus.Areas.Admin.Models;
using Nexus.Identity.Models;
using Nexus.Identity.Models.ManageViewModels;

namespace Nexus.Areas.Admin.Controllers
{
    public class AccountController : AdminBaseController
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger _logger;

        [TempData]
        public string StatusMessage { get; set; }

        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<AccountController> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;            
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> ChangeUserInfo()
        {
            ApplicationUser user = await _userManager.GetUserAsync(User);
            
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var model = new UserInfoViewModel
            {
                Email = user.Email,
                UserName = user.UserName,
                Name = user.Name
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ChangeUserInfo(UserInfoViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                if (model.Email != user.Email)
                {                    
                    string changeEmailToken = await _userManager.GenerateChangeEmailTokenAsync(user, model.Email);
                    IdentityResult changeEmailResult = await _userManager.ChangeEmailAsync(user, model.Email, changeEmailToken);
                    if (!changeEmailResult.Succeeded)
                    {
                        AddErrors(changeEmailResult);
                        return View(model);
                    }
                }

                if (model.UserName != user.UserName)
                {
                    IdentityResult setUsernameResult = await _userManager.SetUserNameAsync(user, model.UserName);
                    if (!setUsernameResult.Succeeded)
                    {
                        AddErrors(setUsernameResult);
                        return View(model);
                    }
                }

                // IdentityUser sınıfında (yani ondan kalıtım alan ApplicationUser sınıfında)
                // custom property (Name) tanımladığımdan .UpdateAsync metodunu çağırmam gerekir.
                user.Name = model.Name;
                await _userManager.UpdateAsync(user);

                SetClientSideNotificationMessage("Your profile information has been updated");
                return RedirectToAction(nameof(ChangeUserInfo));
            }
            else
            {
                ModelState.AddModelError("", "User not found!");
            }

            return View(model);
        }


        [HttpGet]
        public async Task<IActionResult> ChangePassword()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var model = new ChangePasswordViewModel { StatusMessage = StatusMessage };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var changePasswordResult = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
            if (!changePasswordResult.Succeeded)
            {
                AddErrors(changePasswordResult);
                return View(model);
            }

            await _signInManager.SignInAsync(user, isPersistent: false);
            _logger.LogInformation("User changed their password successfully.");
            StatusMessage = "Your password has been changed.";

            return RedirectToAction(nameof(ChangePassword));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            _logger.LogInformation("User logged out.");
            return RedirectToAction(nameof(Nexus.Controllers.HomeController.Index), "Home");
            //return RedirectToAction(nameof(Nexus.Controllers.NotesController.Index), "Notes", new { area = ""});
        }

        #region Helpers
        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }
        #endregion
    }
}