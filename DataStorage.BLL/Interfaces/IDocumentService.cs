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
        Task<IEnumerable<DocumentDTO>> GetAllUserDocumentsAsync(string OwnerId);
        Task<DocumentDTO> GetDocumentByIdAsync(string id);
        Task CreateDocumentAsync(IFormFile uploadedFile, ClaimsPrincipal user, string fdName = null, string parentId = null);
        Task CreateFolderOnRegister(ClaimsPrincipal user);
        void DropFolderOnUserDelete(ClaimsPrincipal user);
        Task UpdateDocumentAsync(DocumentDTO document);
        Task DeleteDocumentAsync(string id);
        bool IfDocumentExists(string id);
        byte[] DownloadFile(string fileId);
    }
}
