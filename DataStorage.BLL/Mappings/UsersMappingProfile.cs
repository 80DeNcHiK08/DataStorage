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
                .ForMember(d => d.UserName, opt => opt.MapFrom(src => src.UserName));

            CreateMap<UserEntity, UserDTO>()
                .ForMember(d => d.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(d => d.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(d => d.UserName, opt => opt.MapFrom(src => src.UserName));
        }
    }
}