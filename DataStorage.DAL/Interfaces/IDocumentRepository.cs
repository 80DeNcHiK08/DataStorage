using DataStorage.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataStorage.DAL.Interfaces
{
    public interface IDocumentRepository
    {
        Task<DocumentEntity> Get(Guid? id);
        Task<bool> Create(DocumentEntity document);
        //Task<bool> Delete(Guid? id);
    }
}
