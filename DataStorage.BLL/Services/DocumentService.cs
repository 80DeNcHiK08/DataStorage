using AutoMapper;
using DataStorage.BLL.DTOs;
using DataStorage.BLL.Interfaces;
using DataStorage.DAL.Entities;
using DataStorage.DAL.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataStorage.BLL.Services
{
    public class DocumentService : IDocumentService
    {
        public IDocumentRepository _docRepo { get; }
        private readonly IMapper _mapper;
        private readonly IPathProvider _pProvider;

        public DocumentService(IDocumentRepository docRepo, IMapper mapper, IPathProvider pProvider)
        {
            _docRepo = docRepo ?? throw new ArgumentNullException(nameof(docRepo));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _pProvider = pProvider ?? throw new ArgumentNullException(nameof(pProvider));
        }
        public async Task<IEnumerable<DocumentDTO>> GetAll()
        {
            var documents = await _docRepo.GetAll();
            var result = _mapper.Map<IEnumerable<DocumentDTO>>(documents);
            return result;
        }
        public async Task<DocumentDTO> Get(Guid? id)
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
        public async Task Create(IFormFile uploadedFile, string id)
        {
            Guid docId = new Guid();
            DocumentDTO docDto = new DocumentDTO {Name = uploadedFile., Length = uploadedFile.Length, IsFile = true, DocumentId = docId};
            await _pProvider.CreateFileOrFolder(uploadedFile, id);
            
            DocumentEntity newDoc = _mapper.Map<DocumentEntity>(docDto);
            await _docRepo.Create(newDoc);
        }
        public async Task Delete(Guid? id)
        {
            await _docRepo.Delete(id);
        }
    }
}
