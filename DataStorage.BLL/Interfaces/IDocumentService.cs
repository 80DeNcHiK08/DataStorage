using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using DataStorage.BLL.DTOs;
using Microsoft.AspNetCore.Http;

namespace DataStorage.BLL.Interfaces
{
    public interface IDocumentService
    {
        Task<IEnumerable<DocumentDTO>> GetAllDocumentsRelatedAsync(string parentId);
        Task<DocumentDTO> GetDocumentByIdAsync(string id);
        Task CreateDocumentRelatedAsync(IFormFileCollection uploadedFile, ClaimsPrincipal user, string parentId, string fdName = null);
        Task CreateFolderOnRegister(string ownerId);
        void DropFolderOnUserDelete(ClaimsPrincipal user);
        Task UpdateDocumentAsync(DocumentDTO document);
        Task DeleteDocumentAsync(string id);
        bool IfDocumentExists(string id);
        string[] GetPathPartsBypId(string fileId);
        string CreateZipFromFolder(string fileId);
        Task DeleteZip(string path);
        Task<DocumentDTO> GetAvailbleDocumentForUserAsync(string link, ClaimsPrincipal user);
        Task<IEnumerable<DocumentDTO>> GetAllAvailbleDocumentsForUserAsync(ClaimsPrincipal user);
        Task<IEnumerable<DocumentDTO>> SearchDocuments(string searchString, string userId);
    }
}
