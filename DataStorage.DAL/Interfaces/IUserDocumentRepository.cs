using System.Threading.Tasks;
using DataStorage.DAL.Entities;

namespace DataStorage.DAL.Interfaces
{
    public interface IUserDocumentRepository
    {
        Task AddUserDocumentAsync(string userId, string documentId, string documentLink = null);
        Task DeleteUserDocumentAsync(string userId, string documentId);
        Task DeleteUserDocumentEntirelyAsync(string documentId);
        // Task<UserDocument> GetUserDocumentByIdAsync(string id);
        // Task<DocumentEntity> GetUserDocumentAsync(string userEmail, string id);
        // Task<string> GetUserDocumentLinkByIdAsync(string id);
    }
}
