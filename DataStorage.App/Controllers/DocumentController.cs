using System;
using System.IO;
using System.Threading.Tasks;
using DataStorage.BLL.DTOs;
using DataStorage.BLL.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DataStorage.App.Controllers
{
    public class DocumentController : Controller
    {
        private readonly IDocumentService _docService;
        IHostingEnvironment _hostingEnvironment;
        private readonly UserManager<UserDTO> _userManager;

        public DocumentController(IHostingEnvironment hostingEnvironment, IDocumentService docService, UserManager<UserDTO> userManager)
        {
            _docService = docService ?? throw new ArgumentNullException(nameof(docService));
            _hostingEnvironment = hostingEnvironment ?? throw new ArgumentNullException(nameof(hostingEnvironment));
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        }

        public IActionResult UserStorage()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddFile(IFormFile uploadedFile)
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            await _docService.Create(uploadedFile, user.Id);
            return RedirectToAction("UserStorage");
        }

        public async Task<IActionResult> DeleteFile()
        {
            //await _docService.Delete();
            return RedirectToAction("UserStorage");
        }
    }
}