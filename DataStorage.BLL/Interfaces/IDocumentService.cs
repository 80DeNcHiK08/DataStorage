using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DataStorage.BLL.DTOs;

namespace DataStorage.BLL.Interfaces
{
    public interface IDocumentService
    {
        Task<DocumentDTO> Get(Guid? id);
        //bool Create(DocumentEntity document);
        //bool Delete(Guid? id);
    }
}
