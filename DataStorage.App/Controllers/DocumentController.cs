using System;
using System.IO;
using System.Threading.Tasks;
using DataStorage.BLL.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DataStorage.App.Controllers
{
    public class DocumentController : Controller
    {
        private readonly IDocumentService _docService;
        IHostingEnvironment _hostingEnvironment;
        public DocumentController(IHostingEnvironment hostingEnvironment, IDocumentService docService)
        {
            _docService = docService ?? throw new ArgumentNullException(nameof(docService));
            _hostingEnvironment = hostingEnvironment;
        }

        public IActionResult UserStorage()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddFile(IFormFile uploadedFile)
        {
            var result = await _docService.Add(uploadedFile);
            return RedirectToAction("UserStorage");
        }
    }
}