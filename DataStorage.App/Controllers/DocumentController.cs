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

namespace DataStorage.App.Controllers
{
    public class DocumentController : Controller
    {
        private readonly IDocumentService _docService;
        private readonly IMapper _mapper;
        private readonly IUsersService _userService;

        public DocumentController(IHostingEnvironment hostingEnvironment, IDocumentService docService, IMapper mapper, IUsersService userService)
        {
            _docService = docService ?? throw new ArgumentNullException(nameof(docService));
            _mapper = mapper;
            _userService = userService;
        }

        [Authorize]
        public async Task<IActionResult> UserStorage(string id)
        {
            if(id == null)
            {
                id = _userService.GetUserId(User);
            }
            await _docService.CreateFolderOnRegister(User);
            return View(_docService.GetAllUserDocumentsAsync(id).Result);
        }

        [Authorize]
        public IActionResult DeleteUser()
        {
            _docService.DropFolderOnUserDelete(User);
            return RedirectToAction("Logout", "Account");
        }

        [HttpPost]
        public async Task<IActionResult> CreateFile(IFormFile uploadedFile)
        {
            await _docService.CreateDocumentAsync(uploadedFile, User);
            return RedirectToAction("UserStorage");
        }

        [HttpPost]
        public async Task<IActionResult> CreateFolder(string FolderName)
        {
            await _docService.CreateDocumentAsync(null, User, FolderName);
            return RedirectToAction("UserStorage");
        }

        public async Task<IActionResult> DeleteFile(string fileId)
        {
            await _docService.DeleteDocumentAsync(fileId);
            return RedirectToAction("UserStorage");
        }

        public async Task<IActionResult> DownloadFile(string fileId)
        {
            var doc = await _docService.GetDocumentByIdAsync(fileId);
            var bytearray = _docService.DownloadFile(fileId);
            using (var filestream = new FileStream(doc.Path, FileMode.Open))
            {
                return File(bytearray, "application/png", doc.Name);
            }
        }
    }
}