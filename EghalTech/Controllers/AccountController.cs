using EghalTech.Models;
using EghalTech.Repository;
using EghalTech.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Runtime.InteropServices;
using System.Security.Claims;

namespace EghalTech.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;
        private readonly IUserDataCleaner userDataCleaner;
        private readonly IRepository<WishList> wishlistRepository;

        public AccountController(UserManager<User> _userManager,
            SignInManager<User> _signInManager,
            IUserDataCleaner _userDataCleaner)
        {
            userManager = _userManager;
            signInManager = _signInManager;
            userDataCleaner = _userDataCleaner;
        }

        [HttpGet]
        public IActionResult Register()
        {
            if (User.Identity?.IsAuthenticated == true)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel userModel)
        {
            if (ModelState.IsValid)
            {
                User dbUser = await userManager.FindByEmailAsync(userModel.Email);
                if (dbUser != null && dbUser.IsDeleted == true)
                {
                    return await ReActivateDeletedUser(dbUser, userModel);
                }

                else if (dbUser != null && dbUser.IsDeleted == false)
                {
                    ModelState.AddModelError("", "Email already registered");
                    return View(userModel);
                }

                User user = new User
                {
                    Name = userModel.Name,
                    Email = userModel.Email,
                    UserName = userModel.Email,
                    PhoneNumber = userModel.Phone,
                    Address = userModel.Address
                };

                var result = await userManager.CreateAsync(user, userModel.Password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "Customer");
                    return RedirectToAction("LogIn");
                }
                else
                {
                    foreach (var error in result.Errors)
                        ModelState.AddModelError("", error.Description);
                }
            }
            return View(userModel);
        }
        [HttpGet]
        public IActionResult LogIn()
        {
            if (User.Identity?.IsAuthenticated == true)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogIn(LoginViewModel loginModel)
        {
            if (ModelState.IsValid)
            {
                User appUser = await userManager.FindByEmailAsync(loginModel.Email);

                if (appUser != null)
                {
                    var password = await userManager.CheckPasswordAsync(appUser, loginModel.Password);
                    if (password)
                    {
                        if (!appUser.IsDeleted)
                        {

                            List<Claim> claims = new List<Claim>();
                            claims.Add(new Claim(ClaimTypes.GivenName, appUser.Name));
                            await signInManager.SignInWithClaimsAsync(appUser, loginModel.RememberMe, claims);
                            return RedirectToAction("Index", "Home");
                        }
                    }
                }
                ModelState.AddModelError("", "Incorrect Email or Password");
            }
            return View(loginModel);
        }
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> ReActivateDeletedUser(User dbUser, RegisterViewModel userModel)
        {
            dbUser.IsDeleted = false;
            dbUser.Name = userModel.Name;
            dbUser.PhoneNumber = userModel.Phone;
            dbUser.Address = userModel.Address;

            var token = await userManager.GeneratePasswordResetTokenAsync(dbUser);
            var resetResult = await userManager.ResetPasswordAsync(dbUser, token, userModel.Password);
            if (!resetResult.Succeeded)
            {
                foreach (var error in resetResult.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View("Register", userModel);
            }

            var updateResult = await userManager.UpdateAsync(dbUser);
            if (!updateResult.Succeeded)
            {
                foreach (var error in updateResult.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View("Register", userModel);
            }

            return RedirectToAction("Login");
        }

        [Authorize]
        public async Task<IActionResult> Profile()
        {
            var user = await userManager.GetUserAsync(User);

            var viewModel = new UserProfileViewModel
            {
                Email = user.Email,
                Name = user.Name,
                PhoneNumber = user.PhoneNumber,
                Address = user.Address,
                ReviewsCount = user.Reviews?.Count ?? 0,
                TotalOrders = user.Orders?.Count ?? 0,
                WishlistItems = user.WishList?.WishlistItems?.Count ?? 0
            };

            return View(viewModel);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteAccount()
        {
            var user = await userManager.GetUserAsync(User);
            userDataCleaner.DeleteUserDataAsync(user);

            user.IsDeleted = true;
            var result = await userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {

                ModelState.AddModelError(string.Empty, "Something went wrong while deleting the account.");
                return RedirectToAction("Profile", "Account");
            }

            await signInManager.SignOutAsync();

            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        public async Task<IActionResult> Edit()
        {
            var user = await userManager.GetUserAsync(User);

            var viewModel = new EditProfileViewModel
            {
                Email = user.Email,
                Name = user.Name,
                PhoneNumber = user.PhoneNumber,
                Address = user.Address
            };
            return View(viewModel);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditProfileViewModel userModel)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.GetUserAsync(User);
                user.Name = userModel.Name;
                user.PhoneNumber = userModel.PhoneNumber;
                user.Address = userModel.Address;

                if (user.Email != userModel.Email)
                {
                    var emailToken = await userManager.GenerateChangeEmailTokenAsync(user, userModel.Email);
                    var emailChangeResult = await userManager.ChangeEmailAsync(user, userModel.Email, emailToken);
                    if (!emailChangeResult.Succeeded)
                    {
                        foreach (var error in emailChangeResult.Errors)
                            ModelState.AddModelError("", error.Description);
                        return View(userModel);
                    }
                }

                var result = await userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    TempData["SuccessMessage"] = "Your profile has been updated successfully.";
                    return RedirectToAction("Profile");
                }
                else
                {
                    foreach (var error in result.Errors)
                        ModelState.AddModelError("", error.Description);
                    return View(userModel);
                }
            }

            return View(userModel);
        }

        [Authorize]
        public IActionResult ChangePassword()
        {
            return View();
        }
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.GetUserAsync(User);
                var result = await userManager.ChangePasswordAsync(user, viewModel.CurrentPassword, viewModel.NewPassword);
                if (result.Succeeded)
                {
                    TempData["SuccessMessage"] = "Your password changed successfully";
                    return RedirectToAction("Profile");
                }
                else
                {
                    foreach (var error in result.Errors)
                        ModelState.AddModelError("", error.Description);
                }
            }
            return View(viewModel);
        }
    }
}
