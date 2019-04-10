using DataStorage.DAL.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataStorage.DAL.Interfaces
{
    public interface IDocumentRepository
    {
        Task<IEnumerable<DocumentEntity>> GetAll();
        Task<DocumentEntity> Get(Guid? id);
        Task<IEnumerable<DocumentEntity>> GetChildren(Guid? id);
        Task Create(DocumentEntity uploadedFile);
        Task Delete(Guid? id);
    }
}
