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
        private readonly IMapper _mapper;
        private string _path;

        public PathProvider(IHostingEnvironment hostingEnvironment, 
            IMapper mapper)
        {
            _hostingEnvironment = hostingEnvironment ?? throw new ArgumentNullException(nameof(hostingEnvironment));
            _path = Path.Combine(_hostingEnvironment.ContentRootPath, "localstorage");
            Directory.CreateDirectory(_path);
            _mapper = mapper;
        }

        public string ContentPath()
        {
            return _path;
        }

        public void CreateFolderOnRegister(string ownerId)
        {
            var endpath = Path.Combine(_path, ownerId);
            if (!File.Exists(endpath))
            {
                Directory.CreateDirectory(endpath);
            }
        }

        public async Task CreateFile(IFormFile file, string endPath)
        {
            using(var fileStream = new FileStream(endPath, FileMode.Create))
            {Â
                await file.CopyToAsync(fileStream);
            }
        }

        public void DeleteFile(string filePath)
        {
            //if (File.Exists(filePath))
            //{
                File.Delete(filePath);
            //}
        }
    }
}