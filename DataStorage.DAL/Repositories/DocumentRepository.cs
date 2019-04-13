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

        public async Task<IEnumerable<DocumentEntity>> GetAll(string OwnerId)
        {
            return await _context.Documents.Where(d => d.OwnerId == Guid.Parse(OwnerId)).ToListAsync();
        }

        public async Task Create(DocumentEntity document)
        {
            var doc = _context.Documents.Where(d => d.DocumentId == document.DocumentId);
            if(doc.Count() == 0)
            {
                var newdoc = await _context.Documents.AddAsync(document);
                await _context.SaveChangesAsync();
            }
        }
        /*public async Task<DocumentEntity> Get(Guid? id)
        {
            var result = await _context.Documents.FirstOrDefaultAsync(f => f.DocumentId == id);
            return result;
        }
        public async Task<IEnumerable<DocumentEntity>> GetChildren(Guid? id)
        {
            var result = await _context.Documents.Where(docid => docid.ParentId == id).ToListAsync();
            return result;
        }
        
        public async Task Delete(Guid? id)
        {
            var founddoc = await _context.Documents.FirstOrDefaultAsync(docId => docId.DocumentId == id);
            _context.Remove(founddoc);
            await _context.SaveChangesAsync();
        }*/
    }
}
