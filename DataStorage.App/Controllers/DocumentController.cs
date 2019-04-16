using System;
using System.IO;
using System.Threading.Tasks;
using DataStorage.BLL.DTOs;
using DataStorage.BLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DataStorage.App.Controllers
{
    public class DocumentController : Controller
    {
        private readonly IDocumentService _docService;

        public DocumentController(IHostingEnvironment hostingEnvironment, IDocumentService docService)
        {
            _docService = docService ?? throw new ArgumentNullException(nameof(docService));
        }

        [Authorize]
        public IActionResult UserStorage()
        {
            _docService.CreateFolderOnRegister(User);
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateFile(IFormFile uploadedFile)
        {
            await _docService.Create(uploadedFile, User);
            return RedirectToAction("UserStorage");
        }

        /*public async Task<IActionResult> DeleteFile()
        {
            //await _docService.Delete();
            return RedirectToAction("UserStorage");
        }*/
    }
}