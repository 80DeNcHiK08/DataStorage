using System.Threading.Tasks;
using DataStorage.DAL.Entities;

namespace DataStorage.DAL.Interfaces
{
    public interface IUserDocumentRepository
    {
        Task CreateAsync(UserDocument document);
        Task DeleteAsync(UserDocument document);
        Task<UserDocument> GetUserDocumentByIdAsync(string id);
        Task<UserDocument> GetUserDocumentAsync(string userEmail, string id);
        Task<string> GetUserDocumentLinkByIdAsync(string id);
    }
}
