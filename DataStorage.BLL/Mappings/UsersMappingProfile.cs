using AutoMapper;
using DataStorage.DAL.Entities;
using DataStorage.BLL.DTOs;

namespace DataStorage.BLL.Mappings
{
    public class UsersMapping : Profile
    {
        public UsersMapping()
        {
            CreateMap<UserDTO, UserEntity>()
                .ForMember(d => d.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(d => d.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(d => d.UserName, opt => opt.MapFrom(src => src.UserName));
            //.ForMember(d => d.Password, opt => opt.MapFrom(src => src.Password));

            CreateMap<UserEntity, UserDTO>()
                .ForMember(d => d.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(d => d.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(d => d.UserName, opt => opt.MapFrom(src => src.UserName));
            //.ForMember(d => d.Password, opt => opt.MapFrom(src => src.Password));
        }
    }
}