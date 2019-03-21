using AutoMapper;

using DataStorage.BLL.ViewModels;
using DataStorage.DAL.Entities;
using DataStorage.DAL.Dtos;


namespace DataStorage.App.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<UserEntity, UserDto>();
            CreateMap<UserDto, UserEntity>();
            CreateMap<RegisterViewModel, UserEntity>();
        }
    }
}
