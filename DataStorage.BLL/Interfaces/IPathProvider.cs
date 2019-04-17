using System.Threading.Tasks;
using DataStorage.BLL.DTOs;
using Microsoft.AspNetCore.Http;

namespace DataStorage.BLL.Interfaces
{
    public interface IPathProvider
    {
        Task CreateFolderOnRegister(string ownerId);
        Task CreateFile(IFormFile file, string endPath);
        string ContentPath();
        void DeleteFile(string filePath);
    }
}