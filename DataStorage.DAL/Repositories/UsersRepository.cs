using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using DataStorage.DAL.Interfaces;
using DataStorage.DAL.Entities;
using Microsoft.AspNetCore.Hosting;

namespace DataStorage.DAL.Repositories
{
    public class UsersRepository : IUsersRepository
    {
        private readonly UserManager<UserEntity> _userManager;
        private readonly SignInManager<UserEntity> _signInManager;
        private readonly PathProvider _pProvider;
        public UsersRepository(UserManager<UserEntity> userManager, SignInManager<UserEntity> signInManager, PathProvider pProvider)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _pProvider = pProvider;
        }

        public async Task<IdentityResult> CreateUserAsync(string userEmail, string userPassword)
        {
            UserEntity user = new UserEntity { Email = userEmail, UserName = userEmail};
            var result = await _userManager.CreateAsync(user, userPassword);
            _pProvider.CreateFolderOnRegister(user.Id.ToString(), user);
            return result;
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