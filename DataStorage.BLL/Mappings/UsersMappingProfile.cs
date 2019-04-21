using AutoMapper;
using DataStorage.DAL.Entities;
using DataStorage.BLL.DTOs;

namespace DataStorage.BLL.Mappings
{
    public class UsersMappingProfile : Profile
    {
        public UsersMappingProfile()
        {
            CreateMap<UserDTO, UserEntity>()
                .ForMember(d => d.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(d => d.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(d => d.UserName, opt => opt.MapFrom(src => src.UserName))
                .ForMember(d => d.StorageSize, opt => opt.MapFrom(src => src.StorageSize))
                .ForMember(d => d.RemainingStorageSize, opt => opt.MapFrom(src => src.RemainingStorageSize))
                .ForMember(d => d.FirstName, opt => opt.MapFrom(src => src.FirstName))
                .ForMember(d => d.LastName, opt => opt.MapFrom(src => src.LastName));

        CreateMap<UserEntity, UserDTO>()
                .ForMember(d => d.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(d => d.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(d => d.UserName, opt => opt.MapFrom(src => src.UserName))
                .ForMember(d => d.StorageSize, opt => opt.MapFrom(src => src.StorageSize))
                .ForMember(d => d.RemainingStorageSize, opt => opt.MapFrom(src => src.RemainingStorageSize))
                .ForMember(d => d.FirstName, opt => opt.MapFrom(src => src.FirstName))
                .ForMember(d => d.LastName, opt => opt.MapFrom(src => src.LastName));
            //.ForMember(d => d.Password, opt => opt.MapFrom(src => src.Password));
        }
    }
}