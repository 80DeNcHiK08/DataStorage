using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using DataStorage.DAL.Interfaces;
using DataStorage.DAL.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace DataStorage.DAL.Repositories
{
    public class UsersRepository : IUsersRepository
    {
        private readonly UserManager<UserEntity> _userManager;
        private readonly SignInManager<UserEntity> _signInManager;
        public UsersRepository(UserManager<UserEntity> userManager, SignInManager<UserEntity> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<IdentityResult> CreateUserAsync(string userEmail, string userPassword)
        {
            UserEntity user = new UserEntity { Email = userEmail, UserName = userEmail};
            return await _userManager.CreateAsync(user, userPassword);
            
        }

        public async Task<SignInResult> GetUserAsync(string userEmail, string userPassword, bool rememberMe)
        {
            return await _signInManager.PasswordSignInAsync(userEmail, userPassword, rememberMe, false);
        }

        public async Task LogOut()
        {
            await _signInManager.SignOutAsync();
        }
    }
}