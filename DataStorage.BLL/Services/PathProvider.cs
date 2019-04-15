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
        private string _path;
        private readonly IMapper _mapper;

        public PathProvider(IHostingEnvironment hostingEnvironment, ApplicationContext context, IMapper mapper)
        {
            _hostingEnvironment = hostingEnvironment ?? throw new ArgumentNullException(nameof(hostingEnvironment));
            _context = context;
            _path = Path.Combine(_hostingEnvironment.ContentRootPath, "../localStorage");
            Directory.CreateDirectory(_path);
            _mapper = mapper;
        }

        public async Task CreateFolderOnRegister(string ownerId)
        {
            var endpath = Path.Combine(_path, ownerId);
            
            DocumentEntity document = new DocumentEntity();
            document.DocumentId = Guid.Parse(ownerId);
            document.Name = ownerId;
            document.IsFile = false;
            document.Path = endpath;
            document.Length = 0;
            document.ParentId = Guid.Empty;
            document.OwnerId = Guid.Parse(ownerId);

            await _context.Documents.AddAsync(document);
            Directory.CreateDirectory(endpath);
            await _context.SaveChangesAsync();
        }

        public async Task CreateFile(IFormFile file, string ownerId)
        {
            string docpath = Path.Combine(_path, ownerId);
            using(var fileStream = new FileStream(docpath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }
        }
    }
}