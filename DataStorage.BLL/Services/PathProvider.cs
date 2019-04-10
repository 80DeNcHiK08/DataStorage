using System;
using System.IO;
using System.Threading.Tasks;
using AutoMapper;
using DataStorage.BLL.DTOs;
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
            _hostingEnvironment = hostingEnvironment;
            _context = context;
            _path = Path.Combine(_hostingEnvironment.ContentRootPath, "../localStorage");
            Directory.CreateDirectory(_path);
            _mapper = mapper;
        }

        public async Task CreateFolderOnRegister(string path, UserDTO owner)
        {
            var endpath = Path.Combine(_path, path);
            DocumentEntity document = new DocumentEntity();
            document.DocumentId = Guid.Parse(owner.Id);
            document.Name = owner.Id;
            document.IsFile = false;
            document.Path = endpath;
            document.Length = 0;
            document.ParentId = Guid.Empty;
            document.Owner = _mapper.Map<UserEntity>(owner);

            await _context.Documents.AddAsync(document);
            await _context.SaveChangesAsync();

            Directory.CreateDirectory(endpath);
        }

        public async Task CreateFileOrFolder(IFormFile file = null, string id = "")
        {
            if(file != null){
                string docpath = _path + "/" + id;
                using(var fileStream = new FileStream(_path, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }
            }
            else {

            }
        }
    }
}