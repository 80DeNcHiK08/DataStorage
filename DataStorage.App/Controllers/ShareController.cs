using System;
using System.Threading.Tasks;
using DataStorage.BLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DataStorage.App.Controllers
{
    public class ShareController : Controller
    {
        private readonly IDocumentService _documentService;
        private readonly ISharingService _sharingService;

        public ShareController(IDocumentService documentService, ISharingService sharingService)
        {
            _documentService = documentService ?? throw new ArgumentNullException(nameof(documentService));
            _sharingService = sharingService ?? throw new ArgumentNullException(nameof(sharingService));
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Get(string link)
        {
            var document = await _sharingService.GetPublicDocumentByLinkAsync(link);
            return View(document);
        }
    }
}