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
        Task<IEnumerable<DocumentDTO>> GetAll(string OwnerId);
        //Task<DocumentDTO> Get(Guid? id);
        //Task<IEnumerable<DocumentDTO>> GetChildren(Guid? id);
        Task Create(IFormFile uploadedFile, ClaimsPrincipal user, string fdName = null, string parentId = null);
        Task CreateFolderOnRegister(ClaimsPrincipal user);
        //Task Delete(Guid? id);
    }
}
