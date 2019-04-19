using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using DataStorage.App.ViewModels;
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
        private readonly IDocumentService _documentService;
        private readonly ISharingService _sharingService;
        private readonly IEmailService _emailService;

        public DocumentController(IDocumentService documentService, ISharingService sharingService, IEmailService emailService)
        {
            _documentService = documentService ?? throw new ArgumentNullException(nameof(documentService));
            _sharingService = sharingService ?? throw new ArgumentNullException(nameof(sharingService));
            _emailService = emailService ?? throw new ArgumentNullException(nameof(emailService));
        }

        [Authorize]
        public IActionResult UserStorage(string id = null)
        {
            _documentService.CreateFolderOnRegister(User);
            ViewBag.ParentId = id;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateFile(IFormFile uploadedFile)
        {
            await _documentService.Create(uploadedFile, User);
            return RedirectToAction("UserStorage");
        }

        [HttpPost]
        public async Task<IActionResult> CreateFolder(string FolderName)
        {
            await _documentService.Create(null, User, FolderName);
            return RedirectToAction("UserStorage");
        }

        [HttpGet]
        public async Task<IActionResult> OpenPublicAccess(string documentId)
        {
            var link = $"{Request.Host.Value}/Share/Get?link={await _sharingService.OpenPublicAccess(documentId, User)}";

            return Content(link);
        }

        [HttpGet]
        public async Task<IActionResult> ClosePublicAccess(string documentId)
        {
            await _sharingService.ClosePublicAccess(documentId, User);
            return Ok();
        }

        [HttpGet]
        public IActionResult OpenLimitedAccessForUser(string documentId)
        {
            if (documentId == null)
            {
                return View("Error");
            }

            return View("OpenLimitedAccess", new SharingViewModel { DocumentId = documentId });
        }


        [HttpPost]
        public async Task<IActionResult> OpenLimitedAccessForUser(SharingViewModel model)
        {
            if (ModelState.IsValid)
            {
                var link = await _sharingService.OpenLimitedAccess(model.DocumentId, User, model.Email);
                var callbackUrl = Url.FileAccessLink(link, Request.Scheme);
                await _emailService.SendEmailAsync(model.Email, "You have been granted an access to the file",
                    $"{User.Identity.Name} has shared a <a href='{callbackUrl}'>file</a> with you");

                return RedirectToAction("UserStorage", "Document");
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> GetAvailbleDocument(string link)
        {
            var document = await _documentService.GetAvailbleDocumentForUserAsync(link, User);
            return View(document);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAvailbleDocuments()
        {
            var availbleDocuments = await _documentService.GetAllAvailbleDocumentsForUserAsync(User);

            return View(availbleDocuments);
        }

        [HttpGet]
        public IActionResult CloseLimitedAccessForUser(string documentId)
        {
            if (documentId == null)
            {
                return View("Error");
            }

            return View("CloseLimitedAccess", new SharingViewModel { DocumentId = documentId });
        }


        [HttpPost]
        public async Task<IActionResult> CloseLimitedAccessForUser(SharingViewModel model)
        {
            if (ModelState.IsValid)
            {
                await _sharingService.CloseLimitedAccessForUser(model.DocumentId, User, model.Email);

                return Ok();
            }

            return View(model);
        }

        public async Task<IActionResult> CloseLimitedAccessEntirely(string documentId)
        {
            await _sharingService.CloseLimitedAccessEntirely(documentId, User);
            return Ok();
        }

        /*public async Task<IActionResult> DeleteFile()
        {
            //await _docService.Delete();
            return RedirectToAction("UserStorage");
        }*/
    }
}