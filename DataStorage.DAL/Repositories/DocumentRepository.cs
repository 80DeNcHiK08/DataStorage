using System;
using System.Linq;
using System.Threading.Tasks;
using DataStorage.DAL.Entities;
using DataStorage.DAL.Interfaces;
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

        public async Task<DocumentEntity> Get(Guid? id)
        {
            var result = await _context.Documents.FirstOrDefaultAsync(f => f.DocumentId == id);
            return result;
        }

        public async Task<Guid> Create(DocumentEntity document)
        {
            var doc = _context.Documents.Where(d => d.Name == document.Name && d.ParentId == document.ParentId);
            if(doc.Count() == 0)
            {
                var newdoc = await _context.Documents.AddAsync(document);
                await _context.SaveChangesAsync();
                return newdoc.Entity.DocumentId;
            }
            return doc.FirstOrDefaultAsync().Result.DocumentId;
        }

        /*public async Task<bool> Delete(Guid? id)
        {
            await _context.Documents.Re
            _context.SaveChanges();
            return true;
        }*/
    }
}
