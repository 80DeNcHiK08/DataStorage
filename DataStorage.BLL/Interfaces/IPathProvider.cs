using System.Threading.Tasks;
using DataStorage.BLL.DTOs;
using Microsoft.AspNetCore.Http;

namespace DataStorage.BLL.Interfaces
{
    public interface IPathProvider
    {
        void CreateFolderOnRegister(string ownerId);
        void DropFolderOnUserDelete(string ownerId);
        Task CreateFile(IFormFile file, string endPath);
        void CreateDirectory(string path);
        string ContentPath();
        void DeleteFile(string filePath);
        string FolderToZip(string filepath);
    }
}