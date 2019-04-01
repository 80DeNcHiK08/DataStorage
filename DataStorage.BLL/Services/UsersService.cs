using AutoMapper;
using DataStorage.BLL.Interfaces;
using DataStorage.DAL.Interfaces;
using DataStorage.BLL.DTOs;
using System.Threading.Tasks;
using System;

namespace DataStorage.BLL
{
    public class UserService : IUsersService
    {
        private readonly IMapper _mapper;
        public IUsersRepository _usersRepo { get; }

        public UserService(IMapper mapper, IUsersRepository usersRepo)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _usersRepo = usersRepo ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<UserDTO> GetUserAsync(string userEmail, string userPassword)
        {
            var user = await _usersRepo.GetUserAsync(userEmail, userPassword);

            return _mapper.Map<UserDTO>(user);
        }

        public async Task<UserDTO> CreateUserAsync(string userEmail, string userPassword)
        {
            var user = await GetUserAsync(userEmail, userPassword);
            if (user == null)
            {
                await _usersRepo.CreateUserAsync(userEmail, userPassword);
                return user;
            }
            else
            {
                return null;
            }
        }
    }
}