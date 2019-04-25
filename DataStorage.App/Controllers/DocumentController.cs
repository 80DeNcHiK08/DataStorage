using System;
using System.Threading.Tasks;
using DataStorage.App.ViewModels;
using DataStorage.BLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DataStorage.App.Controllers
{
    public class DocumentController : Controller
    {
        private readonly IDocumentService _documentService;
        private readonly IUsersService _userService;
        private readonly ISharingService _sharingService;
        private readonly IEmailService _emailService;

        public DocumentController(
            IDocumentService documentService,
            IUsersService userService,
            ISharingService sharingService,
            IEmailService emailService)
        {
            _documentService = documentService ?? throw new ArgumentNullException(nameof(documentService));
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            _emailService = emailService ?? throw new ArgumentNullException(nameof(emailService));
            _sharingService = sharingService ?? throw new ArgumentNullException(nameof(sharingService));
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> UserStorage(string parentId)
        {
            if (parentId == null)
            {
                parentId = _userService.GetUserId(User);
            }
            ViewData["parentId"] = parentId;
            await _documentService.CreateFolderOnRegister(_userService.GetUserId(User));
            return View(_documentService.GetAllDocumentsRelatedAsync(parentId).Result);
        }

        [Authorize]
        public IActionResult DeleteUser()
        {
            _documentService.DropFolderOnUserDelete(User);
            return RedirectToAction("Logout", "Account");
        }

        [HttpPost]
        public async Task<IActionResult> CreateFile(IFormFileCollection uploadedFile, string parentId)
        {
            await _documentService.CreateDocumentRelatedAsync(uploadedFile, User, parentId);
            return RedirectToAction("UserStorage");
        }

        [HttpPost]
        public async Task<IActionResult> CreateFolder(string FolderName, string parentId)
        {
            await _documentService.CreateDocumentRelatedAsync(null, User, parentId, FolderName);
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

                return RedirectToAction("UserStorage", "Documents");
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> LimitedAccessForUser(string DocumentId, string Email)
        {
            //if (ModelState.IsValid)
            //{
            //if (Request.Form["OpenAccess"].ToString() != null)
            //{


            var link = await _sharingService.OpenLimitedAccess(DocumentId, User, Email);//��� ��� ��������

            var callbackUrl = Url.FileAccessLink(link, Request.Scheme);
            await _emailService.SendEmailAsync(Email, "You have been granted an access to the file",
                $"{User.Identity.Name} has shared a <a href='{callbackUrl}'>file</a> with you");

            //return RedirectToAction("ShareFile", "Document", new { fileId = model.DocumentId });
            //}
            /*if (Request.Form["CloseAccess"].ToString() != null)
            {
                await _sharingService.CloseLimitedAccessForUser(model.DocumentId, User, model.Email);
                //return RedirectToAction("ShareFile", "Document", new { fileId = model.DocumentId });
            }*/
            return RedirectToAction("ShareFile", "Document", new { fileId = DocumentId });
        }

        // return View(model);
        // }

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

        // [HttpPost]
        // public async Task<IActionResult> CloseLimitedAccessEntirely(string documentId)
        // {
        //     await _sharingService.CloseLimitedAccessEntirely(documentId, User);
        //     return Ok();
        // }

        public async Task<IActionResult> DeleteFile(string fileId)
        {
            await _documentService.DeleteDocumentAsync(fileId);
            ViewData["parentId"] = null;
            return RedirectToAction("UserStorage");
        }

        public IActionResult ShareFile(string fileId)
        {
            var link = $"{Request.Host.Value}/Share/Get?link={ _sharingService.OpenPublicAccess(fileId, User)}";
            //var model = new ShareViewModel { DocumentId = fileId, ShareLink = link };
            return View(new ShareViewModel { DocumentId = fileId, ShareLink = link, Email = null }
                );
        }

        public async Task<IActionResult> DownloadFile(string fileId)
        {
            var file = await _documentService.GetDocumentByIdAsync(fileId);
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

        public async Task<IActionResult> SearchDocuments(string searchString)
        {
            string userId = _userService.GetUserId(User);
            if (searchString == null)
            {
                return View("UserStorage", _documentService.GetAllDocumentsRelatedAsync(userId).Result);
            }
            var availbleDocuments = await _documentService.SearchDocuments(searchString, userId);
            return View("UserStorage", availbleDocuments);
        }

    }
}