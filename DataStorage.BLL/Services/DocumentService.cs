using AutoMapper;
using DataStorage.BLL.DTOs;
using DataStorage.BLL.Interfaces;
using DataStorage.DAL.Entities;
using DataStorage.DAL.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataStorage.BLL.Services
{
    public class DocumentService : IDocumentService
    {
        public IDocumentRepository _docRepo { get; }
        private readonly IMapper _mapper;

        public DocumentService(IDocumentRepository docRepo, IMapper mapper)
        {
            _docRepo = docRepo ?? throw new ArgumentNullException(nameof(docRepo));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        /*public async Task<IEnumerable<DocumentDTO>> GetAll()
        {
            var documents = await _docRepo.GetAll();
            foreach(var document in documents)
            {
                var result = _mapper.Map<DocumentEntity, DocumentDTO>(document);
            }
            return ;
        }*/
        public async Task<DocumentDTO> Get(Guid? id)
        {
            var document = await _docRepo.Get(id);
            var result = _mapper.Map<DocumentEntity, DocumentDTO>(document);
            return result;
        }
        /*public async List<DocumentDTO> GetChildren(Guid? id)
        {

        }*/
        public async Task<DocumentDTO> Create(IFormFile uploadedFile)
        {
            Guid docId = new Guid();
            DocumentDTO docDto = new DocumentDTO {Name = uploadedFile.Name, Length = uploadedFile.Length, IsFile = true, DocumentId = docId};
            DocumentEntity newDoc = _mapper.Map<DocumentDTO, DocumentEntity>(docDto);
            var result =await _docRepo.Create(newDoc);
            return docDto;
        }
        public async Task Delete(Guid? id)
        {
            await _docRepo.Delete(id);
        }
    }
}
