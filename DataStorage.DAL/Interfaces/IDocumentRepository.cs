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
        Task<DocumentEntity> GetDocumentByIdAsync(string id);
        //Task<IEnumerable<DocumentEntity>> GetChildren(Guid? id);
        Task CreateDocumentAsync(DocumentEntity uploadedFile);
        //Task Delete(Guid? id);
    }
}
