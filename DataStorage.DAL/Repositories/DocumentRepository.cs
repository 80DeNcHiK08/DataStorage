using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataStorage.DAL.Entities;
using DataStorage.DAL.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace DataStorage.DAL.Repositories
{
    public class DocumentRepository : IDocumentRepository
    {
        private readonly ApplicationContext _context;
        public DocumentRepository(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<DocumentEntity>> GetAllUserDocumentsAsync(string OwnerId)
        {
            return await _context.Documents.Where(d => d.OwnerId == OwnerId).ToListAsync();
        }

        public async Task CreateDocumentAsync(DocumentEntity document)
        {
            var doc = _context.Documents.Where(d => d.DocumentId == document.DocumentId);
            if(doc.Count() == 0)
            {
                var newdoc = await _context.Documents.AddAsync(document);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<DocumentEntity> GetDocumentByIdAsync(string id)
        {
            return await _context.Documents.Where(f => f.DocumentId == id).FirstOrDefaultAsync();
        }

        public async Task UpdateDocumentAsync(DocumentEntity document)
        {
            _context.Documents.Update(document);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteDocumentAsync(string id)
        {
            _context.Documents.Remove(GetDocumentByIdAsync(id).Result);
            await _context.SaveChangesAsync();
        }

        public string GetDocumentPathById(string id)
        {
            return GetDocumentByIdAsync(id).Result.Path;
        }
        /*public async Task<IEnumerable<DocumentEntity>> GetChildren(Guid? id)
        {
            var result = await _context.Documents.Where(docid => docid.ParentId == id).ToListAsync();
            return result;
        }*/
    }
}
