using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace DataStorage.App.Controllers
{
    public class DocumentController : Controller
    {
        IHostingEnvironment _hostingEnvironment;
        public DocumentController(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        
    }
}