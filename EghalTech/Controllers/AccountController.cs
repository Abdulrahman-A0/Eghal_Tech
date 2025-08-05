using EghalTech.Models;
using EghalTech.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Security.Claims;

namespace EghalTech.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;

        public AccountController(UserManager<User> _userManager, SignInManager<User> _signInManager)
        {
            userManager = _userManager;
            signInManager = _signInManager;
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
    }
}
