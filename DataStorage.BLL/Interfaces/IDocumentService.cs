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
        Task CreateDocumentRelatedAsync(IFormFile uploadedFile, ClaimsPrincipal user, string parentId, string fdName = null);
        Task CreateFolderOnRegister(string ownerId);
        Task UpdateDocumentAsync(DocumentDTO document);
        Task DeleteDocumentAsync(string id);
        bool IfDocumentExists(string id);
        byte[] DownloadFile(string fileId);
        string[] GetPathPartsBypId(string fileId);
    }
}
