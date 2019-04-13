using DataStorage.BLL.Interfaces;
using DataStorage.DAL.Interfaces;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Identity;
using DataStorage.BLL.DTOs;
using AutoMapper;
using DataStorage.DAL.Entities;

namespace DataStorage.BLL.Services
{
    public class UserService : IUsersService
    {
        private readonly IMapper _mapper;
        private readonly IUsersRepository _usersRepo;

        public UserService(IMapper mapper, IUsersRepository usersRepo)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _usersRepo = usersRepo ?? throw new ArgumentNullException(nameof(usersRepo));
        }

        public async Task<SignInResult> SignInUserAsync(string userEmail, string userPassword, bool rememberMe)
        {
            var user = await _usersRepo.SignInUserAsync(userEmail, userPassword, rememberMe);
            return user;
        }

        public async Task SignInUserAsync(UserDTO user, bool isPersistent)
        {
            var userEntity = _mapper.Map<UserEntity>(user);
            await _usersRepo.SignInUserAsync(userEntity, isPersistent);
        }

        public async Task<string> GetEmailTokenAsync(UserDTO user)
        {
            var userEntity = _mapper.Map<UserEntity>(user);
            return await _usersRepo.GetEmailTokenAsync(userEntity);
        }

        public async Task<IdentityResult> CreateUserAsync(string userEmail, string userPassword)
        {
            return await _usersRepo.CreateUserAsync(userEmail, userPassword);
        }

        public async Task<UserDTO> GetUserByNameAsync(string userEmail)
        {
            var userEntity = await _usersRepo.GetUserByNameAsync(userEmail);
            return _mapper.Map<UserDTO>(userEntity);
        }

        public async Task<bool> IsEmailConfirmedAsync(UserDTO user)
        {
            var userEntity = _mapper.Map<UserEntity>(user);
            return await _usersRepo.IsEmailConfirmedAsync(userEntity);
        }

        public async Task<IdentityResult> ConfirmEmailAsync(string userId, string token)
        {
            var user = await _usersRepo.GetUserByIdAsync(userId);
            return await _usersRepo.ConfirmEmailAsync(user, token);
        }

        public async Task<UserDTO> GetUserByIdAsync(string userId)
        {
            var userEntity = await _usersRepo.GetUserByIdAsync(userId);
            return _mapper.Map<UserDTO>(userEntity);
        }

        public void LogOut()
        {
            _usersRepo.LogOut();
        }
    }
}