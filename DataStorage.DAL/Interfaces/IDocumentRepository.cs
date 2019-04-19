using DataStorage.DAL.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataStorage.DAL.Interfaces
{
    public interface IDocumentRepository
    {
        Task<IEnumerable<DocumentEntity>> GetAllUserDocumentsAsync(string OwnerId);
        // Task<bool> IsDocumentExistsAsync(string id);
        Task<bool> IsUserDocumentOwner(string id, string userId);
        string GenerateAccessLink();
        Task<DocumentEntity> GetDocumentByIdAsync(string id);
        Task<DocumentEntity> GetPublicDocumentByLinkAsync(string link);
        Task UpdateDocumentAsync(DocumentEntity document);
        Task CreateDocumentAsync(DocumentEntity uploadedFile);
        Task<UserDocument> GetAvailbleDocumentForUserAsync(string documentId, string userId);
        Task<IEnumerable<UserDocument>> GetAllAvailbleDocumentsForUserAsync(string userId);
        //Task<IEnumerable<DocumentEntity>> GetChildren(Guid? id);
        //Task Delete(Guid? id);
    }
}
