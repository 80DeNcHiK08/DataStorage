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
        [HttpGet]
        public async Task<IActionResult> UserStorage(string parentId)
        {
            if(parentId == null)
            {
                parentId = _userService.GetUserId(User);
            }
            ViewData["parentId"] = parentId;
            await _docService.CreateFolderOnRegister(_userService.GetUserId(User));
            return View(_docService.GetAllDocumentsRelatedAsync(parentId).Result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateFile(IFormFile uploadedFile, string parentId)
        {
            await _docService.CreateDocumentRelatedAsync(uploadedFile, User, parentId);
            return RedirectToAction("UserStorage");
        }

        [HttpPost]
        public async Task<IActionResult> CreateFolder(string FolderName, string parentId)
        {
            await _docService.CreateDocumentRelatedAsync(null, User, parentId ,FolderName);
            return RedirectToAction("UserStorage");
        }

        public async Task<IActionResult> DeleteFile(string fileId)
        {
            await _docService.DeleteDocumentAsync(fileId);
            return RedirectToAction("UserStorage");
        }

        public async Task<IActionResult> DownloadFile(string fileId)
        {
            var file = await _docService.GetDocumentByIdAsync(fileId);
            var filename = file.Name;
            var file_path = file.Path;
            var file_extention = file.Name.Split(".");
            return PhysicalFile(file_path, "application/" + file_extention[file_extention.Length - 1], filename);
        }

        public async Task<IActionResult> DownloadFolder(string fileId)
        {
            //var folder = await _docService.GetDocumentByIdAsync(fileId);
            //var foldername = folder.Name;
            return RedirectToAction("UserStorage");
        }
    }
}