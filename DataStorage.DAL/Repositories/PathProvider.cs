using System.IO;
using System.Threading.Tasks;
using DataStorage.DAL.Interfaces;
using Microsoft.AspNetCore.Hosting;

namespace DataStorage.DAL.Repositories
{
    public class PathProvider : IPathProvider
    {
        private IHostingEnvironment _hostingEnvironment;
        private string _path;
        public PathProvider(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
            _path = Path.Combine(_hostingEnvironment.ContentRootPath, "localStorage");
            Directory.CreateDirectory(_path);
        }

        public string GetRootPath()
        {
            return _path;
        }

        public void CreateFolder(string path)
        {
            Directory.CreateDirectory(_path + "/" + path);
        }
    }
}