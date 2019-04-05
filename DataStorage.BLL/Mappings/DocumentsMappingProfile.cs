using AutoMapper;
using DataStorage.DAL.Entities;
using DataStorage.BLL.DTOs;

namespace DataStorage.BLL.Mappings
{
    public class DocumentsMappingProfile : Profile
    {
        public DocumentsMappingProfile()
        {
            CreateMap<DocumentDTO, DocumentEntity>()
                .ForMember(d => d.DocumentId, opt => opt.MapFrom(src => src.DocumentId))
                .ForMember(d => d.Name, opt => opt.MapFrom(src => src.Name));

            CreateMap<DocumentEntity, DocumentDTO>()
                .ForMember(d => d.DocumentId, opt => opt.MapFrom(src => src.DocumentId))
                .ForMember(d => d.Name, opt => opt.MapFrom(src => src.Name));
                
        }
    }
}