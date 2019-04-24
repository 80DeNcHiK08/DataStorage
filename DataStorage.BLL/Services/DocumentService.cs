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
        private readonly List<DocumentEntity> _result_list;

        public DocumentService(
            IDocumentRepository documentRepo,
            IMapper mapper,
            IPathProvider pProvider,
            IUsersRepository userRepo)
        {
            _documentRepo = documentRepo ?? throw new ArgumentNullException(nameof(documentRepo));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _pProvider = pProvider ?? throw new ArgumentNullException(nameof(pProvider));
            _userRepo = userRepo ?? throw new ArgumentNullException(nameof(userRepo));
            _result_list = new List<DocumentEntity>();
        }

        public async Task<IEnumerable<DocumentDTO>> GetAllDocumentsRelatedAsync(string parentId)
        {
            var documents = await _documentRepo.GetAllDocumentsRelatedAsync(parentId);
            // var documents = await _documentRepo.GetAllUserDocumentsAsync(OwnerId);
            return _mapper.Map<IEnumerable<DocumentDTO>>(documents);
        }

        public async Task CreateDocumentRelatedAsync(IFormFileCollection uploadedFile, ClaimsPrincipal user, string parentId, string fdName = null)
        {
            if (uploadedFile != null)
            {
                foreach(var file in uploadedFile)
                {
                    string docId = Guid.NewGuid().ToString();
                    string storagePath = Path.Combine(_pProvider.ContentPath());
                    string combinedFilePath = "";
                    foreach (var str in GetPathPartsBypId(parentId))
                    {
                        combinedFilePath += str + "\\";
                    }
                    combinedFilePath.Substring(combinedFilePath.Length - 2);
                    var filePath = Path.Combine(storagePath, combinedFilePath, docId);
                    DocumentDTO docDto = new DocumentDTO
                    {
                        Name = file.FileName,
                        Length = file.Length,
                        IsFile = true,
                        DocumentId = docId,
                        OwnerId = _userRepo.GetUserId(user),
                        ParentId = parentId,
                        Path = filePath
                    };
                    await _pProvider.CreateFile(file, Path.Combine(storagePath, _userRepo.GetUserId(user), docId));
                    DocumentEntity newDoc = _mapper.Map<DocumentEntity>(docDto);
                    await _documentRepo.CreateDocumentAsync(newDoc);
                }
            } else
            {
                string docId = Guid.NewGuid().ToString();
                string storagePath = Path.Combine(_pProvider.ContentPath());
                string combinedFilePath = "";
                foreach (var str in GetPathPartsBypId(parentId))
                {
                    combinedFilePath += str + "\\";
                }
                combinedFilePath.Substring(combinedFilePath.Length - 2);
                var filePath = Path.Combine(storagePath, combinedFilePath, docId);
                DocumentDTO docDto = new DocumentDTO
                {
                    Name = fdName,
                    Length = 0,
                    IsFile = false,
                    DocumentId = docId,
                    OwnerId = _userRepo.GetUserId(user),
                    ParentId = parentId,
                    Path = filePath
                };
                UpdateFolderLength(docDto.ParentId);
                DocumentEntity newDoc = _mapper.Map<DocumentEntity>(docDto);
                await _documentRepo.CreateDocumentAsync(newDoc);
            }
        }

        private void UpdateFolderLength(string parentId)
        {
            var folder = GetDocumentByIdAsync(parentId);
            folder.Result.Length = 0;
            var folder_content = GetAllDocumentsRelatedAsync(parentId);
            foreach(var content in folder_content.Result)
            {
                folder.Result.Length += content.Length;
            }
            _documentRepo.UpdateDocumentAsync(_mapper.Map<DocumentEntity>(folder.Result));
        }

        public async Task CreateFolderOnRegister(string ownerId)
        {
            var endpath = Path.Combine(_pProvider.ContentPath(), ownerId);
            if (GetDocumentByIdAsync(ownerId).Result == null)
            {
                DocumentEntity document = new DocumentEntity
                {
                    DocumentId = ownerId,
                    Name = ownerId,
                    IsFile = false,
                    Path = endpath,
                    Length = 0,
                    ParentId = string.Empty,
                    OwnerId = ownerId
                };
                await _documentRepo.CreateDocumentAsync(document);
            }
            _pProvider.CreateFolderOnRegister(ownerId);
        }

        public async void DropFolderOnUserDelete(ClaimsPrincipal user)
        {
            var ownerId = _userRepo.GetUserId(user);
            var oId = GetDocumentByIdAsync(ownerId);
            await _userRepo.LogOut();
            if (oId != null)
            {
                _pProvider.DropFolderOnUserDelete(ownerId);
                await _documentRepo.DeleteAllUserDocumentsAsync(ownerId);
                _userRepo.DeleteUserAsync(ownerId);
            }
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

        public async Task UpdateDocumentAsync(DocumentDTO document)
        {
            var newDoc = _mapper.Map<DocumentEntity>(document);
            await _documentRepo.UpdateDocumentAsync(newDoc);
        }

        public async Task DeleteDocumentAsync(string id)
        {
            var doc = await _documentRepo.GetDocumentByIdAsync(id);
            if (doc.IsFile)
            {
                _pProvider.DeleteFile(doc.Path);
                await _documentRepo.DeleteDocumentAsync(doc.DocumentId);
            }
            else
            {
                var to_delete = await _documentRepo.GetAllUserDocumentsAsync(doc.OwnerId);
                foreach(var document in to_delete)
                {
                    if(document.Path.IndexOf(doc.Path) > -1)
                    {
                        _pProvider.DeleteFile(document.Path);
                        await _documentRepo.DeleteDocumentAsync(document.DocumentId);
                    }
                }
                _pProvider.DeleteFile(doc.Path);
                await _documentRepo.DeleteDocumentAsync(doc.DocumentId);
            }

        }

        public bool IfDocumentExists(string id)
        {
            var EntityResult = _documentRepo.GetDocumentByIdAsync(id);
            if (EntityResult != null)
            {
                return true;
            } else
                return false;
        }

        public async Task DeleteAllFiles(string ownerId)
        {
            await _documentRepo.DeleteAllUserDocumentsAsync(ownerId);
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
    }
}
