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

        public async Task<IEnumerable<DocumentEntity>> GetAllDocumentsRelatedAsync(string parentId)
        {
            return await _context.Documents.Where(d => d.ParentId == parentId && d.ParentId != string.Empty).ToListAsync();
        }

        public async Task<IEnumerable<DocumentEntity>> GetAllUserDocumentsAsync(string ownerId)
        {
            return await _context.Documents.Where(d => d.OwnerId == ownerId).ToListAsync();
        }

        public async Task DeleteAllUserDocumentsAsync(string ownerId)
        {
            _context.Documents.RemoveRange(_context.Documents.Where(d => d.OwnerId == ownerId));
            await _context.SaveChangesAsync();
        }

        public async Task CreateDocumentAsync(DocumentEntity document)
        {
            var doc = _context.Documents.Where(d => d.DocumentId == document.DocumentId);
            if (doc.Count() == 0)
            {
                await _context.Documents.AddAsync(document);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<DocumentEntity> GetDocumentByIdAsync(string id)
        {
            return await _context.Documents.FirstOrDefaultAsync(d => d.DocumentId == id);
        }

        public async Task<bool> IsUserDocumentOwner(string documentId, string userId)
        {
            var document = await _context.Documents.FirstOrDefaultAsync(doc => doc.DocumentId == documentId && doc.OwnerId == userId);

            if (document != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task UpdateDocumentAsync(DocumentEntity document)
        {
            _context.Documents.Update(document);
            await _context.SaveChangesAsync();
        }

        public async Task<DocumentEntity> GetPublicDocumentByLinkAsync(string link)
        {
            return await _context.Documents.Where(doc => doc.DocumentLink == link && doc.IsPublic == true).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<DocumentEntity>> GetAllAvailbleDocumentsForUserAsync(string userId)
        {
            var user = await _context.Users.Include(u => u.UserDocuments).ThenInclude(ud => ud.Document).FirstOrDefaultAsync(u => u.Id == userId);

            return user.UserDocuments.Where(ud => ud.UserId == userId && ud.DocumentLink != null).Select(ud => ud.Document);
        }

        public async Task<IEnumerable<DocumentEntity>> SearchDocuments(string searchString, string userId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);

            return _context.Documents.Where(doc => doc.Name.Contains(searchString) && doc.OwnerId==userId);
        }

        public async Task<DocumentEntity> GetAvailbleDocumentForUserAsync(string link, string userId)
        {
            var user = await _context.Users.Include(u => u.UserDocuments).ThenInclude(ud => ud.Document).FirstOrDefaultAsync(u => u.Id == userId);

            return user.UserDocuments.FirstOrDefault(ud => ud.DocumentLink == link).Document;
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

        public bool CheckPublic(string documentId)
        {
            var doc = _context.Documents.FirstOrDefault(d => d.DocumentId == documentId).IsPublic;
            return doc;
        }

        public List<string> GetAllUsersWithAccess(string documentId)
        {
            var document = _context.Documents.Include(d => d.UserDocuments).ThenInclude(ud => ud.User).FirstOrDefault(d => d.DocumentId == documentId);
            var emailList = new List<string>();
            foreach (UserDocument userDocument in document.UserDocuments)
            {
                if (document.OwnerId != userDocument.UserId)
                {
                    emailList.Add(userDocument.User.Email);
                }
            }
            return emailList;
        }
    }
}
