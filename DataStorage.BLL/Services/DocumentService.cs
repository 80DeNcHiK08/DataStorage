using AutoMapper;
using DataStorage.BLL.DTOs;
using DataStorage.BLL.Interfaces;
using DataStorage.DAL.Entities;
using DataStorage.DAL.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DataStorage.BLL.Services
{
    public class DocumentService : IDocumentService
    {
        private readonly IDocumentRepository _documentRepo;
        private readonly IUserDocumentRepository _userDocumentRepo;
        private readonly IMapper _mapper;
        private readonly IPathProvider _pProvider;
        private readonly IUsersRepository _userRepo;
        private readonly List<DocumentEntity> _result_list;

        public DocumentService(
            IDocumentRepository documentRepo,
            IMapper mapper,
            IPathProvider pProvider,
            IUsersRepository userRepo,
            IUserDocumentRepository userDocumentRepo)
        {
            _documentRepo = documentRepo ?? throw new ArgumentNullException(nameof(documentRepo));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _pProvider = pProvider ?? throw new ArgumentNullException(nameof(pProvider));
            _userRepo = userRepo ?? throw new ArgumentNullException(nameof(userRepo));
            _userDocumentRepo = userDocumentRepo ?? throw new ArgumentNullException(nameof(userDocumentRepo));
            _result_list = new List<DocumentEntity>();
        }

        public async Task<IEnumerable<DocumentDTO>> GetAllDocumentsRelatedAsync(string parentId)
        {
            var documents = await _documentRepo.GetAllDocumentsRelatedAsync(parentId);
            return _mapper.Map<IEnumerable<DocumentDTO>>(documents);
        }
        public async Task CreateDocumentRelatedAsync(IFormFileCollection uploadedFile, ClaimsPrincipal owner, string parentId, string fdName = null)
        {
            string storagePath = Path.Combine(_pProvider.ContentPath());
            string combinedFilePath = "";
            string ownerId = _userRepo.GetUserId(owner);
            if (uploadedFile != null)
            {
                foreach (var file in uploadedFile)
                {
                    if(_documentRepo.GetDocumentByIdAsync(parentId).Result.Length <= _userRepo.GetUserByIdAsync(ownerId).Result.StorageSize)
                    {
                        string docId = Guid.NewGuid().ToString();
                        foreach (var str in GetPathPartsBypId(parentId))
                        {
                            combinedFilePath += str + "\\";
                        }
                        combinedFilePath.Substring(combinedFilePath.Length - 2);
                        var filePath = Path.Combine(storagePath, combinedFilePath, docId);
                        var doc = new DocumentEntity
                        {
                            Name = file.FileName,
                            Length = file.Length,
                            IsFile = true,
                            DocumentId = docId,
                            OwnerId = ownerId,
                            ParentId = parentId,
                            Path = filePath,
                            ChangeDate = DateTime.Now
                        };

                        await _documentRepo.CreateDocumentAsync(doc);
                        await _userDocumentRepo.AddUserDocumentAsync(ownerId, docId);
                        await _pProvider.CreateFile(file, Path.Combine(storagePath, _userRepo.GetUserId(owner), docId));
                        await UpdateFolderLength(doc.ParentId);
                    }
                }
            } else if (fdName != null) {
                string docId = Guid.NewGuid().ToString();
                foreach (var str in GetPathPartsBypId(parentId))
                {
                    combinedFilePath += str + "\\";
                }
                combinedFilePath.Substring(combinedFilePath.Length - 2);
                var filePath = Path.Combine(storagePath, combinedFilePath, docId);
                var doc = new DocumentEntity
                {
                    Name = fdName,
                    Length = 0,
                    IsFile = false,
                    DocumentId = docId,
                    OwnerId = ownerId,
                    ParentId = parentId,
                    Path = filePath,
                    ChangeDate = DateTime.Now
                };

                await _documentRepo.CreateDocumentAsync(doc);
                await _userDocumentRepo.AddUserDocumentAsync(ownerId, docId);
            }
        }

        public async Task UpdateFolderLength(string parentId)
        {
            var folder = await _documentRepo.GetDocumentByIdAsync(parentId);
            folder.Length = 0;
            var folder_content = GetAllDocumentsRelatedAsync(parentId);
            foreach (var content in folder_content.Result)
            {
                folder.Length += content.Length;
            }
            await _documentRepo.UpdateDocumentAsync(folder);
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
                // await _userDocumentRepo.AddUserDocumentAsync(ownerId, ownerId);
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
                await _userRepo.DeleteUserAsync(ownerId);
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
            var document = await _documentRepo.GetAvailbleDocumentForUserAsync(link, userId);

            return _mapper.Map<DocumentDTO>(document);
        }

        public async Task<IEnumerable<DocumentDTO>> GetAllAvailbleDocumentsForUserAsync(ClaimsPrincipal user)
        {
            string userId = _userRepo.GetUserId(user);
            var availbleDocuments = await _documentRepo.GetAllAvailbleDocumentsForUserAsync(userId);
            var documents = new List<DocumentEntity>();

            foreach (var item in availbleDocuments)
            {
                documents.Add(item);
            }

            return _mapper.Map<IEnumerable<DocumentDTO>>(documents);
        }

        public async Task<IEnumerable<DocumentDTO>> SearchDocuments(string searchString, string userId)
        {
            var availbleDocuments = await _documentRepo.SearchDocuments(searchString, userId);
            return _mapper.Map<IEnumerable<DocumentDTO>>(availbleDocuments); ;
        }

        public async Task UpdateDocumentAsync(DocumentDTO document)
        {
            var newDoc = _mapper.Map<DocumentEntity>(document);
            await _documentRepo.UpdateDocumentAsync(newDoc);
        }

        public async Task DeleteDocumentAsync(string id)
        {
            var doc = await _documentRepo.GetDocumentByIdAsync(id);
            string storagePath = Path.Combine(_pProvider.ContentPath(), doc.OwnerId);
            if (doc.IsFile)
            {
                _pProvider.DeleteFile(Path.Combine(storagePath, doc.DocumentId));
                await _documentRepo.DeleteDocumentAsync(doc.DocumentId);
            }
            else
            {
                var to_delete = await _documentRepo.GetAllUserDocumentsAsync(doc.OwnerId);
                foreach (var document in to_delete)
                {
                    if (document.Path.Contains(doc.Path))
                    {
                        if(document.IsFile)
                        {
                            _pProvider.DeleteFile(Path.Combine(storagePath, document.DocumentId));
                        }
                        await _documentRepo.DeleteDocumentAsync(document.DocumentId);
                    }
                }
                await _documentRepo.DeleteDocumentAsync(doc.DocumentId);
            }

        }

        public bool IfDocumentExists(string id)
        {
            var EntityResult = _documentRepo.GetDocumentByIdAsync(id);
            if (EntityResult != null)
            {
                return true;
            }
            else
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
            while (doc.Result.ParentId != string.Empty)
            {
                var prev_doc = GetDocumentByIdAsync(doc.Result.ParentId);
                result.Add(prev_doc.Result.DocumentId);
                doc = GetDocumentByIdAsync(doc.Result.ParentId);
            }
            result.Reverse();
            return result.ToArray();
        }

        public bool CheckPublic(string documentId)
        {
            return _documentRepo.CheckPublic(documentId);
        }

        public List<string> GetAllUsersWithAccess(string documentId)
        {
            return _documentRepo.GetAllUsersWithAccess(documentId);
        }
        public string CreateZipFromFolder(string path)
        {
            return "";
        }
    }
}
