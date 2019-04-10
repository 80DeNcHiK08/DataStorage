using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System;
using Microsoft.AspNetCore.Authorization;

using DataStorage.BLL.Interfaces;
using DataStorage.App.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace DataStorage.App.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUsersService _userService;

        public AccountController(IUsersService userService)
        {
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var login = await _userService.GetUserAsync(model.Email, model.Password, model.rememberMe);
                if (login.Succeeded)
                {
                    return RedirectToAction("UserStorage", "Document");
                }
                else
                {
                    ViewData["UserDoesNotExistErrorMessage"] = "Incorrect username and/or password";
                    ModelState.AddModelError("", "Incorrect username and/or password");
                }
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
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var register = await _userService.CreateUserAsync(model.Email, model.Password);
                if (register.Succeeded)
                {
                    await _userService.GetUserAsync(model.Email, model.Password, true);

                    return RedirectToAction("UserStorage", "Document");
                }
                else
                {
                    ViewData["UserExistErrorMessage"] = "Incorrect username and/or password";
                    ModelState.AddModelError("", "Incorrect username and/or password");
                }
            }
            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            await _userService.LogOut();
            return RedirectToAction("Welcome", "Home");
        }
    }    
}