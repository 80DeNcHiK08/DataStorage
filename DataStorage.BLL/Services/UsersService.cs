using DataStorage.BLL.Interfaces;
using DataStorage.DAL.Interfaces;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using DataStorage.BLL.DTOs;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace DataStorage.BLL.Services
{
    public class UserService : IUsersService
    {
        public IUsersRepository _usersRepo { get; }
        private readonly IPathProvider _pProvider;

        private readonly IHttpContextAccessor _httpContextAccessor;
        public UserService(IUsersRepository usersRepo, IPathProvider pProvider, IHttpContextAccessor httpContextAccessor)
        {
            _usersRepo = usersRepo ?? throw new ArgumentNullException(nameof(usersRepo));
            _pProvider = pProvider ?? throw new ArgumentNullException(nameof(pProvider));
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<SignInResult> GetUserAsync(string userEmail, string userPassword, bool rememberMe)
        {
            return await _usersRepo.GetUserAsync(userEmail, userPassword, rememberMe);
        }

        public async Task<IdentityResult> CreateUserAsync(string userEmail, string userPassword)
        {
            var result = await _usersRepo.CreateUserAsync(userEmail, userPassword);
            await _pProvider.CreateFolderOnRegister(_httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            return result;
        }

        public async Task LogOut()
        {
            await _usersRepo.LogOut();
        }

        public string GetCurrentUserId()
        {
            return _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
        }

    }
}