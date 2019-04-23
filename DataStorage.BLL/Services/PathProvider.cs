using System;
using System.IO;
using System.Threading.Tasks;
using AutoMapper;
using DataStorage.BLL.Interfaces;
using DataStorage.DAL;
using DataStorage.DAL.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DataStorage.BLL.Services
{
    public class PathProvider : IPathProvider
    {
        private IHostingEnvironment _hostingEnvironment;
        private readonly IMapper _mapper;
        private string _path;
        private string _pathfortempzip;

        public PathProvider(IHostingEnvironment hostingEnvironment,
            IMapper mapper)
        {
            _hostingEnvironment = hostingEnvironment ?? throw new ArgumentNullException(nameof(hostingEnvironment));
            _path = Path.Combine(_hostingEnvironment.ContentRootPath, "localstorage");
            _pathfortempzip = Path.Combine(_hostingEnvironment.ContentRootPath, "tempZip");
            Directory.CreateDirectory(_path);
            Directory.CreateDirectory(_pathfortempzip);
            _mapper = mapper;
        }

        public string ContentPath()
        {
            return _path;
        }

        public void CreateDirectory(string path)
        {
            Directory.CreateDirectory(path);
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
            using (var fileStream = new FileStream(endPath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }
        }

        public void DeleteFile(string filePath)
        {
            File.Delete(filePath);
        }

        public string FolderToZip(string filePath)
        {
            return "";
        }
    }
}