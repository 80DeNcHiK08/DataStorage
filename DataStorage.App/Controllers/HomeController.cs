using System;
using System.IO;
using System.Threading.Tasks;
using AutoMapper;
using DataStorage.BLL.DTOs;
using DataStorage.BLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using DataStorage.App.ViewModels;

namespace DataStorage.App.Controllers
{
    public class HomeController : Controller
    {
        private readonly IEmailService _emailService;
        private readonly IUsersService _userService;

        public HomeController(IUsersService userService, IEmailService emailService)
        {
            _emailService = emailService;
            _userService = userService;
        }

        [Authorize]
        public IActionResult UserStorage()
        {
            return View();
        }

        [AllowAnonymous]
        public IActionResult Welcome()
        {
            return View();
        }

        
        [Authorize]
        public async Task<IActionResult> Profile()
        {
            var model = new ProfileViewModel { UserId = _userService.GetUserId(User) };
            var user = await _userService.GetUserByIdAsync(model.UserId);
            model.Token = await _userService.GetResetPasswordTokenAsync(user);
            model.Email = user.Email;
            model.StorageSize = user.StorageSize;
            model.UserFirstName = user.FirstName;
            model.UserLastName = user.LastName;

            return View(model);
        }

        [Authorize]
        public  IActionResult ResetPasswordProf(string userId, string token)
        {
            return RedirectToAction("ResetPassword", "Account", new { userId = userId, token = token });
        }

        [Authorize]
        public async Task<IActionResult> IncreaseStorage()
        {
            var user = await _userService.GetUserByIdAsync(_userService.GetUserId(User));
            var token = await _userService.GetEmailTokenAsync(user);
            var callbackUrl = Url.IncreaseConfirmationLink(user.Id, Request.Scheme);
            await _emailService.SendEmailAsync(user.Email, "Confirm increase",
                        $"Confirm the storage increase by clicking on the <a href='{callbackUrl}'>link</a>");
            return Content("Check the email and click on the link in the letter to increase storage");
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmIncrease(string userId)
        {
            if (userId == null)
            {
                return View("Error");
            }
            var user = await _userService.GetUserByIdAsync(userId);
            if (user == null)
            {
                return View("Error");
            }
            var result = await _userService.ConfirmIncreaseAsync(userId);
            if (result==true)
                return RedirectToAction("Profile", "Home");
            else
                return View("Error");
        }
    }
}
