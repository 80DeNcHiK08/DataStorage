using DataStorage.BLL.Interfaces;
using DataStorage.DAL.Interfaces;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using DataStorage.DAL;
using DataStorage.BLL.DTOs;
using AutoMapper;
using DataStorage.DAL.Entities;
using Microsoft.AspNetCore.Authentication;

namespace DataStorage.BLL.Services
{
    public class UsersService : IUsersService
    {
        public IUsersRepository _usersRepo { get; }
        private readonly IPathProvider _pProvider;
        private readonly IMapper _mapper;

        public UsersService(IUsersRepository usersRepo, IPathProvider pProvider, IMapper mapper)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _usersRepo = usersRepo ?? throw new ArgumentNullException(nameof(usersRepo));
            _pProvider = pProvider ?? throw new ArgumentNullException(nameof(pProvider));
        }

        public async Task<SignInResult> SignInUserAsync(string userEmail, string userPassword, bool rememberMe)
        {
            return await _usersRepo.SignInUserAsync(userEmail, userPassword, rememberMe);;
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
            var result = await _usersRepo.CreateUserAsync(userEmail, userPassword);
            return result;
        }
        
        public async Task<IdentityResult> CreateUserAsync(string userEmail)
        {
            return await _usersRepo.CreateUserAsync(userEmail);
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
            var userEntity = await _usersRepo.GetUserByIdAsync(userId);
            return await _usersRepo.ConfirmEmailAsync(userEntity, token);
        }

        public async Task<UserDTO> GetUserByIdAsync(string userId)
        {
            var userEntity = await _usersRepo.GetUserByIdAsync(userId);
            return _mapper.Map<UserDTO>(userEntity);
        }

        public string GetUserId(ClaimsPrincipal user)
        {
            return _usersRepo.GetUserId(user);
        }

        public async Task<ExternalLoginInfo> GetExternalLoginInfoAsync()
        {
            return await _usersRepo.GetExternalLoginInfoAsync();
        }

        public async Task<SignInResult> ExternalLoginSignInAsync(string loginProvider, string providerKey, bool isPersistent, bool bypassTwoFactor)
        {
            return await _usersRepo.ExternalLoginSignInAsync(loginProvider, providerKey, isPersistent, bypassTwoFactor);
        }

        public async Task<IdentityResult> AddLoginAsync(string userEmail, ExternalLoginInfo loginInfo)
        {
            var user = await _usersRepo.GetUserByNameAsync(userEmail);
            return await _usersRepo.AddLoginAsync(user, loginInfo);
        }

        public AuthenticationProperties ConfigureExternalAuthenticationProperties(string provider, string redirectUrl)
        {
            return _usersRepo.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
        }

        public async Task LogOut()
        {
            await _usersRepo.LogOut();
        }

        public async Task<string> GetResetPasswordTokenAsync(UserDTO user)
        {
            var userEntity = _mapper.Map<UserEntity>(user);
            return await _usersRepo.GetResetPasswordTokenAsync(userEntity);
        }

        public async Task<IdentityResult> ResetPasswordAsync(string userEmail, string token, string newPassword)
        {
            var user = await _usersRepo.GetUserByNameAsync(userEmail);
            return await _usersRepo.ResetPasswordAsync(user, token, newPassword);
        }
    }
}