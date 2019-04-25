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

        public async Task AddUserDocumentAsync(string userId, string documentId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            // var document = await _context.Documents.FirstOrDefaultAsync(d => d.DocumentId == documentId);
            user.UserDocuments.Add(new UserDocument { UserId = user.Id, DocumentId = documentId });
            await _context.SaveChangesAsync();
        }

        public async Task DeleteUserDocumentAsync(UserEntity user, UserDocument document)
        {
            user.UserDocuments.Remove(document);
            await _context.SaveChangesAsync();
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
