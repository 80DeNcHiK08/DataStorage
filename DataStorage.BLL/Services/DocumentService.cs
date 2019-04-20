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
        public async Task<IEnumerable<DocumentDTO>> GetAllDocumentsRelatedAsync(string parentId)
        {
            var documents = await _docRepo.GetAllDocumentsRelatedAsync(parentId);
            return _mapper.Map<IEnumerable<DocumentDTO>>(documents);
        }
        public async Task CreateDocumentRelatedAsync(IFormFile uploadedFile, ClaimsPrincipal user, string parentId, string fdName = null)
        {
            string docId = Guid.NewGuid().ToString();
            string storagePath = Path.Combine(_pProvider.ContentPath());
            string combinedFilePath = "";
            foreach(var str in GetPathPartsBypId(parentId)) {
                combinedFilePath += str + "\\";
            }
            combinedFilePath.Substring(combinedFilePath.Length - 2);
            var filePath = Path.Combine(storagePath, combinedFilePath, docId);
            if (uploadedFile != null)
            {
                DocumentDTO docDto = new DocumentDTO
                {
                    Name = uploadedFile.FileName,
                    Length = uploadedFile.Length,
                    IsFile = true,
                    DocumentId = docId,
                    OwnerId = _userService.GetUserId(user),
                    ParentId = parentId,
                    Path = filePath
                };
                await _pProvider.CreateFile(uploadedFile, Path.Combine(storagePath, _userService.GetUserId(user), docId));

                DocumentEntity newDoc = _mapper.Map<DocumentEntity>(docDto);
                await _docRepo.CreateDocumentAsync(newDoc);
            } else
            {
                DocumentDTO docDto = new DocumentDTO
                {
                    Name = fdName,
                    Length = 0,
                    IsFile = false,
                    DocumentId = docId,
                    OwnerId = _userService.GetUserId(user),
                    ParentId = parentId,
                    Path = filePath
                };

                DocumentEntity newDoc = _mapper.Map<DocumentEntity>(docDto);
                await _docRepo.CreateDocumentAsync(newDoc);
            }
        }

        public async Task CreateFolderOnRegister(string ownerId)
        {
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
            _pProvider.CreateFolderOnRegister(ownerId);
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

        public byte[] DownloadFile(string fileId)
        {
            var doc = _docRepo.GetDocumentByIdAsync(fileId);
            byte[] file = _pProvider.GetFileToArray(doc.Result.Path);
            return file;
        }

        public string[] GetPathPartsBypId(string fileId)
        {
            var doc = GetDocumentByIdAsync(fileId);
            var result = new List<string>();
            result.Add(doc.Result.DocumentId);
            while(doc.Result.ParentId != string.Empty)
            {
                var prev_doc = GetDocumentByIdAsync(doc.Result.ParentId);
                result.Add(prev_doc.Result.DocumentId);
                doc = GetDocumentByIdAsync(doc.Result.ParentId);
            }
            result.Reverse();
            return result.ToArray();
        }
        /*public async Task<IEnumerable<DocumentDTO>> GetChildren(Guid? id)
        {
            var documents = await _docRepo.GetChildren(id);
            var result = _mapper.Map<IEnumerable<DocumentDTO>>(documents);
            return result;
        }*/
    }
}
