using DataStorage.DAL.Entities;

namespace DataStorage.DAL.Interfaces
{
    public interface IPathProvider
    {
        string GetRootPath();
         void CreateFolderOnRegister(string path, UserEntity owner);
         void Create(DocumentEntity document);
    }
}