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
                .ForMember(d => d.OwnerId, opt => opt.MapFrom(src => src.OwnerId))
                // .ForMember(d => d.Owner, opt => opt.MapFrom(src => src.Owner))
                .ForMember(d => d.Length, opt => opt.MapFrom(src => src.Length))
                .ForMember(d => d.DocumentLink, opt => opt.MapFrom(src => src.DocumentLink))
                .ForMember(d => d.ChangeDate, opt => opt.MapFrom(src => src.ChangeDate))
                .ForMember(d => d.IsPublic, opt => opt.MapFrom(src => src.IsPublic))
                .ForMember(d => d.UserDocuments, opt => opt.MapFrom(src => src.UserDocuments));

            CreateMap<DocumentEntity, DocumentDTO>()
                .ForMember(d => d.DocumentId, opt => opt.MapFrom(src => src.DocumentId))
                .ForMember(d => d.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(d => d.Path, opt => opt.MapFrom(src => src.Path))
                .ForMember(d => d.ParentId, opt => opt.MapFrom(src => src.ParentId))
                .ForMember(d => d.IsFile, opt => opt.MapFrom(src => src.IsFile))
                .ForMember(d => d.OwnerId, opt => opt.MapFrom(src => src.OwnerId))
                // .ForMember(d => d.Owner, opt => opt.MapFrom(src => src.Owner))
                .ForMember(d => d.Length, opt => opt.MapFrom(src => src.Length))
                .ForMember(d => d.DocumentLink, opt => opt.MapFrom(src => src.DocumentLink))
                .ForMember(d => d.ChangeDate, opt => opt.MapFrom(src => src.ChangeDate))
                .ForMember(d => d.IsPublic, opt => opt.MapFrom(src => src.IsPublic))
                .ForMember(d => d.UserDocuments, opt => opt.MapFrom(src => src.UserDocuments));
        }
    }
}