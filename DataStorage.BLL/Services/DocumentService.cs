using AutoMapper;
using DataStorage.BLL.DTOs;
using DataStorage.BLL.Interfaces;
using DataStorage.DAL.Entities;
using DataStorage.DAL.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DataStorage.BLL.Services
{
    public class DocumentService : IDocumentService
    {
        public IDocumentRepository _docRepo { get; }
        private readonly IMapper _mapper;
        private readonly IPathProvider _pProvider;
        private readonly UserService _userService;

        public DocumentService(IDocumentRepository docRepo, IMapper mapper, IPathProvider pProvider, UserService userService)
        {
            _docRepo = docRepo ?? throw new ArgumentNullException(nameof(docRepo));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _pProvider = pProvider ?? throw new ArgumentNullException(nameof(pProvider));
            _userService = userService;
        }
        public async Task<IEnumerable<DocumentDTO>> GetAll(string OwnerId)
        {
            var documents = await _docRepo.GetAll(OwnerId);
            return _mapper.Map<IEnumerable<DocumentDTO>>(documents);
        }
        public async Task Create(IFormFile uploadedFile)
        {
            Guid docId = new Guid();
            DocumentDTO docDto = new DocumentDTO {Name = uploadedFile.Name, Length = uploadedFile.Length, IsFile = true, DocumentId = docId};
            await _pProvider.CreateFile(uploadedFile, _userService.GetCurrentUserId());
            
            DocumentEntity newDoc = _mapper.Map<DocumentEntity>(docDto);
            await _docRepo.Create(newDoc);
        }
        /*public async Task<DocumentDTO> Get(Guid? id)
        {
            var document = await _docRepo.Get(id);
            var result = _mapper.Map<DocumentDTO>(document);
            return result;
        }
        public async Task<IEnumerable<DocumentDTO>> GetChildren(Guid? id)
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
