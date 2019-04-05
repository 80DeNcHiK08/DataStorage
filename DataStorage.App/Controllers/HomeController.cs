using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System;

namespace DataStorage.App.Controllers
{
    public class HomeController : Controller
    {
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
    }
}
