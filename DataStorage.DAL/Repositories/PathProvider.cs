using System;
using System.IO;
using System.Threading.Tasks;
using DataStorage.DAL.Entities;
using DataStorage.DAL.Interfaces;
using Microsoft.AspNetCore.Hosting;

namespace DataStorage.DAL.Repositories
{
    public class PathProvider : IPathProvider
    {
        private IHostingEnvironment _hostingEnvironment;
        private ApplicationContext _context;
        private string _path;
        public PathProvider(IHostingEnvironment hostingEnvironment, ApplicationContext context)
        {
            _hostingEnvironment = hostingEnvironment;
            _context = context;
            _path = Path.Combine(_hostingEnvironment.ContentRootPath, "../localStorage");
            Directory.CreateDirectory(_path);
        }

        public string GetRootPath()
        {
            return _path;
        }

        public void CreateFolderOnRegister(string path, UserEntity owner)
        {
            var endpath = Path.Combine(_path, path);
            DocumentEntity document = new DocumentEntity();
            document.DocumentId = Guid.Parse(owner.Id);
            document.Name = owner.Id;
            document.IsFile = false;
            document.Path = endpath;
            _context.Documents.Add(document);
            _context.SaveChanges();

            Directory.CreateDirectory(endpath);
        }
    }
}