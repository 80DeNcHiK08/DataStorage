using AutoMapper;
using DataStorage.BLL.Interfaces;
using DataStorage.DAL.Interfaces;
using DataStorage.BLL.DTOs;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Identity;

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

        public async Task<SignInResult> GetUserAsync(string userEmail, string userPassword, bool rememberMe)
        {
            var user = await _usersRepo.GetUserAsync(userEmail, userPassword, rememberMe);
            _mapper.Map<UserDTO>(user);
            return user;
        }

        public async Task<IdentityResult> CreateUserAsync(string userEmail, string userPassword)
        {
            return await _usersRepo.CreateUserAsync(userEmail, userPassword);
        }

        /*public void LogOut()
        {
            _usersRepo.LogOut();
        }*/
    }
}