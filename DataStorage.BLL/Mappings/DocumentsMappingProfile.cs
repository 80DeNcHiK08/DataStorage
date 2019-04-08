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
                .ForMember(d => d.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(d => d.Path, opt => opt.MapFrom(src => src.Path))
                .ForMember(d => d.ParentId, opt => opt.MapFrom(src => src.ParentId))
                .ForMember(d => d.IsFile, opt => opt.MapFrom(src => src.IsFile))
                .ForMember(d => d.Owner, opt => opt.MapFrom(src => src.Owner));

            CreateMap<DocumentEntity, DocumentDTO>()
                .ForMember(d => d.DocumentId, opt => opt.MapFrom(src => src.DocumentId))
                .ForMember(d => d.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(d => d.Path, opt => opt.MapFrom(src => src.Path))
                .ForMember(d => d.ParentId, opt => opt.MapFrom(src => src.ParentId))
                .ForMember(d => d.IsFile, opt => opt.MapFrom(src => src.IsFile))
                .ForMember(d => d.Owner, opt => opt.MapFrom(src => src.Owner));                
        }
    }
}