using System.Threading.Tasks;
using DataStorage.DAL.Entities;

namespace DataStorage.DAL.Interfaces
{
    public interface IUserDocumentRepository
    {
        Task AddUserDocumentAsync(string userId, string documentId, string documentLink = null);
        Task DeleteUserDocumentAsync(UserEntity user, UserDocument document);
        // Task<DocumentEntity> GetUserDocumentByIdAsync(string id);
        Task<DocumentEntity> GetUserDocumentAsync(string userEmail, string id);
        Task<string> GetUserDocumentLinkByIdAsync(string id);
    }
}
