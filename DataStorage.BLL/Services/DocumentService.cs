using AutoMapper;
using DataStorage.BLL.DTOs;
using DataStorage.BLL.Interfaces;
using DataStorage.DAL.Entities;
using DataStorage.DAL.Interfaces;
using System;
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
            _mapper = mapper;
        }

        public async Task<DocumentDTO> Get(Guid? id)
        {
            var doc = await _docRepo.Get(id);

            return _mapper.Map<DocumentEntity, DocumentDTO>(doc);
        }

        public async Task<DocumentDTO> Add(IFormFile uploadedFile)
        {
            
        }
    }
}
