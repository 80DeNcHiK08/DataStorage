using DataStorage.BLL.Interfaces;
using DataStorage.DAL.Interfaces;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace DataStorage.BLL.Services
{
    public class UsersService : IUsersService
    {
        public IUsersRepository _usersRepo { get; }
        private readonly IPathProvider _pProvider;
        private IHttpContextAccessor _httpContextAccessor;

        public UsersService(IUsersRepository usersRepo, IPathProvider pProvider, IHttpContextAccessor httpContextAccessor)
        {
            _usersRepo = usersRepo ?? throw new ArgumentNullException(nameof(usersRepo));
            _pProvider = pProvider ?? throw new ArgumentNullException(nameof(pProvider));
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }

        public async Task<SignInResult> GetUserAsync(string userEmail, string userPassword, bool rememberMe)
        {
            return await _usersRepo.GetUserAsync(userEmail, userPassword, rememberMe);
        }

        public async Task<IdentityResult> CreateUserAsync(string userEmail, string userPassword)
        {
            var result = await _usersRepo.CreateUserAsync(userEmail, userPassword);
            
            return result;
        }

        public async Task CreateFolderOnRegister()
        {
            await _pProvider.CreateFolderOnRegister(GetCurrentUserId());
        }

        public async Task LogOut()
        {
            await _usersRepo.LogOut();
        }
        
        public string GetCurrentUserId()
        {
           return _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value.ToString();
        }

    }
}