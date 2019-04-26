using System;
using System.IO;
using System.Threading.Tasks;
using DataStorage.App.ViewModels;
using DataStorage.BLL.Interfaces;
using DataStorage.BLL.Services;
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
        private readonly IPathProvider _pProvider;

        public DocumentController(
            IDocumentService documentService,
            IUsersService userService,
            ISharingService sharingService,
            IEmailService emailService,
            IPathProvider pProvider)
        {
            _documentService = documentService ?? throw new ArgumentNullException(nameof(documentService));
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            _emailService = emailService ?? throw new ArgumentNullException(nameof(emailService));
            _sharingService = sharingService ?? throw new ArgumentNullException(nameof(sharingService));
            _pProvider = pProvider;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> UserStorage(string parentId)
        {
            if (parentId == null)
            {
                parentId = _userService.GetUserId(User);
            }
            ViewData["ownerId"] = _userService.GetUserId(User);
            ViewData["parentId"] = parentId;
            await _documentService.CreateFolderOnRegister(_userService.GetUserId(User));
            await _documentService.UpdateFolderLength(_userService.GetUserId(User));
            ViewData["CurentSize"] = _documentService.GetDocumentByIdAsync(_userService.GetUserId(User)).Result.Length.ToString();
            ViewData["StorageSize"] = _userService.GetUserByIdAsync(_userService.GetUserId(User)).Result.StorageSize.ToString();
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
            return Redirect("/Document/UserStorage?parentId=" + parentId);
        }

        [HttpPost]
        public async Task<IActionResult> CreateFolder(string FolderName, string parentId)
        {
            await _documentService.CreateDocumentRelatedAsync(null, User, parentId ,FolderName);
            return Redirect("/Document/UserStorage?parentId=" + parentId);
        }

        [HttpGet]
        public async Task<IActionResult> OpenPublicAccess(string documentId)
        {
            var link = $"{Request.Host.Value}/Document/Get?link={await _sharingService.OpenPublicAccess(documentId, User)}";

            return Content(link);
        }

        [HttpGet]
        public async Task<IActionResult> ClosePublicAccess(string documentId)
        {
            await _sharingService.ClosePublicAccess(documentId, User);
            return Ok();
        }

       /* [HttpGet]
        public IActionResult OpenLimitedAccessForUser(string documentId)
        {
            if (documentId == null)
            {
                return View("Error");
            }

            return View("OpenLimitedAccess", new SharingViewModel { DocumentId = documentId });
        }*/
        
        [HttpPost]
        public async Task<IActionResult> OpenLimitedAccessForUser(SharingViewModel model)
        {
            if (ModelState.IsValid)
            {
                var link = await _sharingService.OpenLimitedAccess(model.DocumentId, User, model.Email);
                if (link != null)
                {
                    var callbackUrl = Url.FileAccessLink(link, Request.Scheme);
                    await _emailService.SendEmailAsync(model.Email, "You have been granted an access to the file",
                        $"{User.Identity.Name} has shared a <a href='{callbackUrl}'>file</a> with you");
                    return RedirectToAction("UserStorage", "Document");
                }

                return View("ShareFile", model);
            }

            return View("ShareFile", model);
        }

        [HttpGet]
        public async Task<IActionResult> GetAvailbleDocument(string link)
        {
            var document = await _documentService.GetAvailbleDocumentForUserAsync(link, User);
            return View("SharedFile", document);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAvailbleDocuments()
        {
            var availbleDocuments = await _documentService.GetAllAvailbleDocumentsForUserAsync(User);

            return View(availbleDocuments);
        }

        /*[HttpGet]
        public IActionResult CloseLimitedAccessForUser(string documentId)
        {
            if (documentId == null)
            {
                return View("Error");
            }

            return View("CloseLimitedAccess", new SharingViewModel { DocumentId = documentId });
        }*/


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

        [HttpPost]
        public async Task<IActionResult> CloseLimitedAccessEntirely(string documentId)
        {
            await _sharingService.CloseLimitedAccessEntirely(documentId, User);
            return Ok();
        }

        public JsonResult DeleteFile(string fileId)
        {
            bool res = false;
            if(fileId != null)
            {
                _documentService.DeleteDocumentAsync(fileId);
            }
            return Json(res);
        }

         public IActionResult ShareFile(string documentId)
         {
            //var link = $"{Request.Host.Value}/Share/Get?link={ _sharingService.OpenPublicAccess(fileId, User)}";
            bool isPublic = _documentService.CheckPublic(documentId);
             return View(new SharingViewModel { DocumentId = documentId, Email = null, IsPublic = isPublic, UsersEmails=_documentService.GetAllUsersWithAccess(documentId)}
                 );
         }


        public IActionResult DownloadFile(string fileId)
        {
            var file = _documentService.GetDocumentByIdAsync(fileId);
            string physical_path = _pProvider.ContentPath() + "\\" + file.Result.OwnerId + "\\" + file.Result.DocumentId;

            if(file.Result.IsFile)
            {
                string contentType = ContentType.GetContentType(file.Result.Name);
                return PhysicalFile(physical_path, contentType, file.Result.Name);
            } else {
                return View();
                //
            }
        }

        public async Task<IActionResult> DeleteAll()
        {
            await _documentService.DeleteAllFiles(User.Identity.Name.ToString());
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

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Get(string link)
        {
            var document = await _sharingService.GetPublicDocumentByLinkAsync(link);
            return View("SharedFile", document);
        }
    }
}