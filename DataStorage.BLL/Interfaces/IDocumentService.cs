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
        Task Create(IFormFile uploadedFile, ClaimsPrincipal user, string fdName = null, string parentId = null);
        Task CreateFolderOnRegister(ClaimsPrincipal user);
        Task<DocumentDTO> GetAvailbleDocumentForUserAsync(string link, ClaimsPrincipal user);
        Task<IEnumerable<DocumentDTO>> GetAllAvailbleDocumentsForUserAsync(ClaimsPrincipal user);
        //Task<IEnumerable<DocumentDTO>> GetChildren(Guid? id);

        //Task Delete(Guid? id);
    }
}
