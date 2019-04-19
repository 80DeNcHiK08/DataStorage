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
        private readonly IDocumentRepository _documentRepo;
        private readonly IMapper _mapper;
        private readonly IPathProvider _pProvider;
        private readonly IUsersRepository _userRepo;
        // private readonly IUserDocumentRepository _userDocumentRepo;

        public DocumentService(IDocumentRepository docRepo, IMapper mapper, IPathProvider pProvider, IUsersRepository userService, IUserDocumentRepository userDocumentRepo)
        {
            _documentRepo = docRepo ?? throw new ArgumentNullException(nameof(docRepo));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _pProvider = pProvider ?? throw new ArgumentNullException(nameof(pProvider));
            _userRepo = userService ?? throw new ArgumentNullException(nameof(userService));
            // _userDocumentRepo = userDocumentRepo ?? throw new ArgumentNullException(nameof(userDocumentRepo));
        }
        public async Task<IEnumerable<DocumentDTO>> GetAllUserDocumentsAsync(string OwnerId)
        {
            var documents = await _documentRepo.GetAllUserDocumentsAsync(OwnerId);
            return _mapper.Map<IEnumerable<DocumentDTO>>(documents);
        }
        public async Task Create(IFormFile uploadedFile, ClaimsPrincipal user, string fdName = null, string parentId = null)
        {
            string docId = Guid.NewGuid().ToString();
            if (uploadedFile != null)
            {
                string endPath = Path.Combine(_pProvider.ContentPath(), _userRepo.GetUserId(user), docId.ToString());
                DocumentDTO docDto = new DocumentDTO
                {
                    Name = uploadedFile.FileName,
                    Length = uploadedFile.Length,
                    IsFile = true,
                    DocumentId = docId,
                    OwnerId = _userRepo.GetUserId(user),
                    ParentId = parentId,
                    Path = endPath
                };
                await _pProvider.CreateFile(uploadedFile, endPath);

                DocumentEntity newDoc = _mapper.Map<DocumentEntity>(docDto);
                await _documentRepo.CreateDocumentAsync(newDoc);
            } else
            {
                string endPath = Path.Combine(_pProvider.ContentPath(), _userRepo.GetUserId(user), docId.ToString());
                DocumentDTO docDto = new DocumentDTO
                {
                    Name = fdName,
                    Length = 0,
                    IsFile = false,
                    DocumentId = docId,
                    OwnerId = _userRepo.GetUserId(user),
                    ParentId = parentId,
                    Path = endPath
                };

                DocumentEntity newDoc = _mapper.Map<DocumentEntity>(docDto);
                await _documentRepo.CreateDocumentAsync(newDoc);
            }
        }

        public async Task CreateFolderOnRegister(ClaimsPrincipal user)
        {
            await _pProvider.CreateFolderOnRegister(_userRepo.GetUserId(user));
        }

        public async Task<DocumentDTO> GetDocumentByIdAsync(string id)
        {
            var document = await _documentRepo.GetDocumentByIdAsync(id);
            var result = _mapper.Map<DocumentDTO>(document);
            return result;
        }

        public async Task<DocumentDTO> GetAvailbleDocumentForUserAsync(string link, ClaimsPrincipal user)
        {
            string userId = _userRepo.GetUserId(user);
            var userDocument = await _documentRepo.GetAvailbleDocumentForUserAsync(link, userId);

            return _mapper.Map<DocumentDTO>(userDocument.Document);
        }

        public async Task<IEnumerable<DocumentDTO>> GetAllAvailbleDocumentsForUserAsync(ClaimsPrincipal user)
        {
            string userId = _userRepo.GetUserId(user);
            var availbleDocuments = await _documentRepo.GetAllAvailbleDocumentsForUserAsync(userId);
            var documents = new List<DocumentEntity>();

            foreach (var item in availbleDocuments)
            {
                documents.Add(item.Document);
            }

            return _mapper.Map<IEnumerable<DocumentDTO>>(documents);
        }




        /*public async Task<IEnumerable<DocumentDTO>> GetChildren(Guid? id)
        {
            var documents = await _docRepo.GetChildren(id);
            var result = _mapper.Map<IEnumerable<DocumentDTO>>(documents);
            return result;
        }
        
        public async Task Delete(Guid? id)
        {
            await _docRepo.Delete(id);
        }*/
    }
}
