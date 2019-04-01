using System.Threading.Tasks;
using DataStorage.DAL.Interfaces;
using DataStorage.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

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

        public async void CreateUserAsync(string userEmail, string userPassword)
        {
            UserEntity user = new UserEntity { Email = userEmail };
            await _userManager.CreateAsync(user, userPassword);
        }

        public async void GetUserAsync(string userEmail, string userPassword, bool rememberMe)
        {
            var result = await _signInManager.PasswordSignInAsync(userEmail, userPassword, rememberMe, false);
        }

        public async void LogOut()
        {
            await _signInManager.SignOutAsync();
        }
    }
}