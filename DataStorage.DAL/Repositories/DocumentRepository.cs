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

        public async Task<bool> Create(DocumentEntity document)
        {
            var ifdocex = await _context.Documents.FindAsync(document);
            if(ifdocex == null)
            {
                var newdoc = await _context.Documents.AddAsync(document);
                //await _context.SaveChangesAsync(document);
                return true;
            }
            return false;
        }

        /*public async Task<bool> Delete(Guid? id)
        {
            await _context.Documents.//RemoveAsync(Get(id));
            _context.SaveChanges();
            return true;
        }*/
    }
}
