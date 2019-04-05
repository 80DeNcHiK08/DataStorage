using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace DataStorage.BLL.Services
{
    public class PathProvider 
    {
        private IHostingEnvironment _hostingEnvironment;
        private string _path;
        public PathProvider(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
            _path = Path.Combine(_hostingEnvironment.WebRootPath, "localStorage");
            Directory.CreateDirectory(_path);
        }
    }
}