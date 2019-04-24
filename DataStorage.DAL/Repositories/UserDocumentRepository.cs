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

        public async Task CreateAsync(UserDocument document)
        {
            await _context.UserDocuments.AddAsync(document);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(UserDocument document)
        {
            _context.UserDocuments.Remove(document);
            await _context.SaveChangesAsync();
        }  
        
        public async Task<UserDocument> GetUserDocumentByIdAsync(string id)
        {
            return await _context.UserDocuments.FirstOrDefaultAsync(u => u.DocumentId == id);
        }

        public async Task<UserDocument> GetUserDocumentAsync(string userEmail, string id)
        {
            return await _context.UserDocuments.FirstOrDefaultAsync(u => u.DocumentId == id && u.User.Email == userEmail);
        }

        public async Task<string> GetUserDocumentLinkByIdAsync(string id)
        {
            var userDocument = await _context.UserDocuments.FirstOrDefaultAsync(u => u.DocumentId == id);

            return userDocument.DocumentLink;
        }
    }
}
