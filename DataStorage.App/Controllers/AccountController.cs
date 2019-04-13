using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DataStorage.BLL.Interfaces;
using DataStorage.App.ViewModels;
using Microsoft.AspNetCore.Authorization;
using System;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;

namespace DataStorage.App.Controllers
{
    public class AccountController : Controller
    {
        private readonly IEmailService _emailService;
        private readonly IUsersService _userService;

        public AccountController(IUsersService userService, IEmailService emailService)
        {
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            _emailService = emailService ?? throw new ArgumentNullException(nameof(emailService));
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userService.GetUserByNameAsync(model.Email);
                if (user != null)
                {
                    if (!await _userService.IsEmailConfirmedAsync(user))
                    {
                        ModelState.AddModelError(string.Empty, "You have not confirmed your mail yet");
                        return View(model);
                    }

                    var login = await _userService.SignInUserAsync(model.Email, model.Password, model.rememberMe);
                    if (login.Succeeded)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }

                ModelState.AddModelError("", "Incorrect username and/or password");
            }

            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var register = await _userService.CreateUserAsync(model.Email, model.Password);
                if (register.Succeeded)
                {
                    var user = await _userService.GetUserByNameAsync(model.Email);

                    var token = await _userService.GetEmailTokenAsync(user);

                    var callbackUrl = Url.EmailConfirmationLink(user.Id, token, Request.Scheme);

                    await _emailService.SendEmailAsync(user.Email, "Confirm your account",
                        $"Confirm the registration by clicking on the <a href='{callbackUrl}'>link</a>");

                    await _userService.SignInUserAsync(user, false);

                    return Content("Check the email and click on the link in the letter to complete the registration");
                }

                ModelState.AddModelError("", "Incorrect username and/or password");
            }

            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (userId == null || token == null)
            {
                return View("Error");
            }
            var user = await _userService.GetUserByIdAsync(userId);
            if (user == null)
            {
                return View("Error");
            }
            var result = await _userService.ConfirmEmailAsync(userId, token);
            if (result.Succeeded)
                return RedirectToAction("Index", "Home");
            else
                return View("Error");
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public IActionResult ExternalLogin()
        {
            var redirectUrl = Url.Action("ExternalLoginCallback", "Account");
            var properties = _userService.ConfigureExternalAuthenticationProperties("Google", redirectUrl);
            return Challenge(properties, "Google");
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ExternalLoginCallback()
        {
            var loginInfo = await _userService.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }

            var email = loginInfo.Principal.FindFirstValue(ClaimTypes.Email);
            var user = await _userService.GetUserByNameAsync(email);

            if (user != null)
            {
                if (!await _userService.IsEmailConfirmedAsync(user))
                {
                    return Content("You have not confirmed your mail yet");
                }

                var result = await _userService.ExternalLoginSignInAsync(loginInfo.LoginProvider, loginInfo.ProviderKey, false, true);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
            }

            var register = await _userService.CreateUserAsync(email);
            if (register.Succeeded)
            {
                register = await _userService.AddLoginAsync(email, loginInfo);
                if (register.Succeeded)
                {
                    var createdUser = await _userService.GetUserByNameAsync(email);

                    var token = await _userService.GetEmailTokenAsync(createdUser);

                    var callbackUrl = Url.EmailConfirmationLink(createdUser.Id, token, Request.Scheme);

                    await _emailService.SendEmailAsync(createdUser.Email, "Confirm your account",
                        $"Confirm the registration by clicking on the <a href='{callbackUrl}'>link</a>");

                    await _userService.SignInUserAsync(createdUser, false);

                    return Content("Check the email and click on the link in the letter to complete the registration");
                }
            }

            return RedirectToAction("Login", "Account");
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            _userService.LogOut();
            return RedirectToAction("Login", "Account");
        }
    }
}