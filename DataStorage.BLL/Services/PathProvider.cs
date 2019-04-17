using System;
using System.IO;
using System.Threading.Tasks;
using AutoMapper;
using DataStorage.BLL.Interfaces;
using DataStorage.DAL;
using DataStorage.DAL.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace DataStorage.BLL.Services
{
    public class PathProvider : IPathProvider
    {
        private IHostingEnvironment _hostingEnvironment;
        private ApplicationContext _context;
        private readonly IMapper _mapper;
        private string _path;

        public PathProvider(IHostingEnvironment hostingEnvironment, ApplicationContext context, IMapper mapper)
        {
            _hostingEnvironment = hostingEnvironment ?? throw new ArgumentNullException(nameof(hostingEnvironment));
            _context = context;
            _path = Path.Combine(_hostingEnvironment.ContentRootPath, "../localStorage");
            Directory.CreateDirectory(_path);
            _mapper = mapper;
        }

        public string ContentPath()
        {
            return _path;
        }

        public async Task CreateFolderOnRegister(string ownerId)
        {
            var endpath = Path.Combine(_path, ownerId);
            if (!File.Exists(endpath))
            {
                DocumentEntity document = new DocumentEntity();
                document.DocumentId = ownerId;
                document.Name = ownerId;
                document.IsFile = false;
                document.Path = endpath;
                document.Size = 0;
                document.ParentId = string.Empty;
                document.OwnerId = ownerId;

                await _context.Documents.AddAsync(document);
                Directory.CreateDirectory(endpath);
                await _context.SaveChangesAsync();
            }
        }

        public async Task CreateFile(IFormFile file, string endPath)
        {
            using(var fileStream = new FileStream(endPath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }
        }

        public void DeleteFile(string filePath)
        {
            using (var fileStream = new FileStream(filePath, FileMode.Open))
            {
                File.Delete(filePath);
            }
        }
    }
}