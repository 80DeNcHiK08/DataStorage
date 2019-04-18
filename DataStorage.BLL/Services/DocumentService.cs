using AutoMapper;
using DataStorage.BLL.DTOs;
using DataStorage.BLL.Interfaces;
using DataStorage.DAL.Entities;
using DataStorage.DAL.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DataStorage.BLL.Services
{
    public class DocumentService : IDocumentService
    {
        public IDocumentRepository _docRepo { get; }
        private readonly IMapper _mapper;
        private readonly IPathProvider _pProvider;
        private readonly IUsersService _userService;

        public DocumentService(IDocumentRepository docRepo, 
            IMapper mapper, 
            IPathProvider pProvider, 
            IUsersService userService)
        {
            _docRepo = docRepo ?? throw new ArgumentNullException(nameof(docRepo));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _pProvider = pProvider ?? throw new ArgumentNullException(nameof(pProvider));
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
        }
        public async Task<IEnumerable<DocumentDTO>> GetAllUserDocumentsAsync(string OwnerId)
        {
            var documents = await _docRepo.GetAllUserDocumentsAsync(OwnerId);
            return _mapper.Map<IEnumerable<DocumentDTO>>(documents);
        }
        public async Task CreateDocumentAsync(IFormFile uploadedFile, ClaimsPrincipal user, string fdName = null, string parentId = null)
        {
            string docId = Guid.NewGuid().ToString();
            if (uploadedFile != null)
            {
                string endPath = Path.Combine(_pProvider.ContentPath(), _userService.GetUserId(user), docId.ToString());
                DocumentDTO docDto = new DocumentDTO
                {
                    Name = uploadedFile.FileName,
                    Length = uploadedFile.Length,
                    IsFile = true,
                    DocumentId = docId,
                    OwnerId = _userService.GetUserId(user),
                    ParentId = parentId,
                    Path = endPath
                };
                await _pProvider.CreateFile(uploadedFile, endPath);

                DocumentEntity newDoc = _mapper.Map<DocumentEntity>(docDto);
                await _docRepo.CreateDocumentAsync(newDoc);
            } else
            {
                string endPath = Path.Combine(_pProvider.ContentPath(), _userService.GetUserId(user), docId.ToString());
                DocumentDTO docDto = new DocumentDTO
                {
                    Name = fdName,
                    Length = 0,
                    IsFile = false,
                    DocumentId = docId,
                    OwnerId = _userService.GetUserId(user),
                    ParentId = parentId,
                    Path = endPath
                };

                DocumentEntity newDoc = _mapper.Map<DocumentEntity>(docDto);
                await _docRepo.CreateDocumentAsync(newDoc);
            }
        }

        public async Task CreateFolderOnRegister(ClaimsPrincipal user)
        {
            var ownerId = user.Identity.Name;
            var endpath = Path.Combine(_pProvider.ContentPath(), ownerId);
            if (GetDocumentByIdAsync(ownerId) == null)
            {
                DocumentEntity document = new DocumentEntity
                {
                    DocumentId = ownerId,
                    Name = ownerId,
                    IsFile = false,
                    Path = endpath,
                    Size = 0,
                    ParentId = string.Empty,
                    OwnerId = ownerId
                };
                await _docRepo.CreateDocumentAsync(document);
            }
            _pProvider.CreateFolderOnRegister(_userService.GetUserId(user));
        }
        public async Task<DocumentDTO> GetDocumentByIdAsync(string id)
        {
            var document = await _docRepo.GetDocumentByIdAsync(id);
            var result = _mapper.Map<DocumentDTO>(document);
            return result;
        }

        public async Task UpdateDocumentAsync(DocumentDTO document)
        {
            var newDoc = _mapper.Map<DocumentEntity>(document);
            await _docRepo.UpdateDocumentAsync(newDoc);
        }

        public async Task DeleteDocumentAsync(string id)
        {
            _pProvider.DeleteFile(_docRepo.GetDocumentPathById(id));
            await _docRepo.DeleteDocumentAsync(id);
        }

        public bool IfDocumentExists(string id)
        {
            var EntityResult = _docRepo.GetDocumentByIdAsync(id);
            if (EntityResult != null)
            {
                return true;
            } else
                return false;
        }
        /*public async Task<IEnumerable<DocumentDTO>> GetChildren(Guid? id)
        {
            var documents = await _docRepo.GetChildren(id);
            var result = _mapper.Map<IEnumerable<DocumentDTO>>(documents);
            return result;
        }*/
    }
}
