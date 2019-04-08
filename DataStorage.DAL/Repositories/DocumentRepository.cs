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

        public async Task<IEnumerable<DocumentEntity>> GetAll()
        {
            var result = await _context.Documents.ToListAsync();
            return result;
        }
        public async Task<DocumentEntity> Get(Guid? id)
        {
            var result = await _context.Documents.FirstOrDefaultAsync(f => f.DocumentId == id);
            return result;
        }
        public async Task<IEnumerable<DocumentEntity>> GetChildren(Guid? id)
        {
            var result = await _context.Documents.Where(docid => docid.DocumentId == id).ToListAsync();
            return result;
        }
        public async Task<DocumentEntity> Create(DocumentEntity document)
        {
            var doc = _context.Documents.Where(d => d.Name == document.Name && d.ParentId == document.ParentId);
            if(doc.Count() == 0)
            {
                var newdoc = await _context.Documents.AddAsync(document);
                await _context.SaveChangesAsync();
                return newdoc.Entity;
            }
            return doc.FirstOrDefaultAsync().Result;
        }

        public async Task Delete(Guid? id)
        {
            var founddoc = await _context.Documents.FirstOrDefaultAsync(docId => docId.DocumentId == id);
            _context.Remove(founddoc);
            await _context.SaveChangesAsync();
        }
    }
}
