using System.Threading.Tasks;
using DataStorage.BLL.DTOs;
using Microsoft.AspNetCore.Http;

namespace DataStorage.BLL.Interfaces
{
    public interface IPathProvider
    {
         Task CreateFolderOnRegister(string path, UserDTO owner);
         Task CreateFileOrFolder(IFormFile file, string id);
    }
}