using DataStorage.DAL.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataStorage.DAL.Interfaces
{
    public interface IDocumentRepository
    {
        Task<IEnumerable<DocumentEntity>> GetAllDocumentsRelatedAsync(string parentId);
        Task<IEnumerable<DocumentEntity>> GetAllUserDocumentsAsync(string OwnerId);
        string GenerateAccessLink();
        Task<bool> IsUserDocumentOwner(string id, string userId);
        Task<DocumentEntity> GetDocumentByIdAsync(string id);
        Task CreateDocumentAsync(DocumentEntity uploadedFile);
        Task UpdateDocumentAsync(DocumentEntity document);
        Task DeleteDocumentAsync(string id);
        string GetDocumentPathById(string id);
        Task<DocumentEntity> GetPublicDocumentByLinkAsync(string link);
        Task<UserDocument> GetAvailbleDocumentForUserAsync(string documentId, string userId);
        Task<IEnumerable<UserDocument>> GetAllAvailbleDocumentsForUserAsync(string userId);
    }
}
