namespace DataStorage.DAL.Interfaces
{
    public interface IPathProvider
    {
        string GetRootPath();
         void CreateFolder(string path);
    }
}