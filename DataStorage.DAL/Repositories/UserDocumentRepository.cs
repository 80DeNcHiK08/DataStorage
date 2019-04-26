using System;
using System.Linq;
using System.Threading.Tasks;
using DataStorage.DAL.Entities;
using DataStorage.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DataStorage.DAL.Repositories
{
    public class UserDocumentRepository : IUserDocumentRepository
    {
        private readonly ApplicationContext _context;

        public UserDocumentRepository(ApplicationContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task AddUserDocumentAsync(string userId, string documentId, string documentLink = null)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            user.UserDocuments.Add(new UserDocument { UserId = user.Id, DocumentId = documentId, DocumentLink = documentLink });
            await _context.SaveChangesAsync();
        }

        public async Task DeleteUserDocumentAsync(string userId, string documentId)
        {
            var user = await _context.Users.Include(u => u.UserDocuments).FirstOrDefaultAsync(u => u.Id == userId);
            var userDocument = user.UserDocuments.FirstOrDefault(ud => ud.DocumentId == documentId);
            user.UserDocuments.Remove(userDocument);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteUserDocumentEntirelyAsync(string documentId)
        {
            var document = await _context.Documents.Include(d => d.UserDocuments).FirstOrDefaultAsync(d => d.DocumentId == documentId);
            for (int i = document.UserDocuments.Count - 1; i >= 1; i--)
            {
                if (document.UserDocuments.ElementAt(i).DocumentLink != null)
                {
                    document.UserDocuments.Remove(document.UserDocuments.ElementAt(i));
                    i++;
                }
            }
        }

        public async Task<DocumentEntity> GetUserDocumentAsync(string userEmail, string id)
        {
            var document = await _context.Documents.FirstOrDefaultAsync(d => d.DocumentId == id);
            var userDocument = document.UserDocuments.FirstOrDefault(ud => ud.User.Email == userEmail);

            return userDocument.Document;
        }

        public async Task<string> GetUserDocumentLinkByIdAsync(string id)
        {
            var userDocument = await _context.Documents.FirstOrDefaultAsync(d => d.DocumentId == id);

            return userDocument.DocumentLink;
        }
    }
}
