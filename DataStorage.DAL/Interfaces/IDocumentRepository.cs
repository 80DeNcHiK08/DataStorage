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
        Task<DocumentEntity> GetDocumentByIdAsync(string id);
        Task CreateDocumentAsync(DocumentEntity uploadedFile);
        Task UpdateDocumentAsync(DocumentEntity document);
        Task DeleteDocumentAsync(string id);
        string GetDocumentPathById(string id);
    }
}
