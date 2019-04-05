using DataStorage.DAL.Interfaces;
using System;

namespace DataStorage.BLL.Services
{
    public class DocumentService
    {
        public IDocumentRepository _docRepo { get; }

        public DocumentService(IDocumentRepository docRepo)
        {
            _docRepo = docRepo ?? throw new ArgumentNullException(nameof(docRepo));
        }


    }
}
