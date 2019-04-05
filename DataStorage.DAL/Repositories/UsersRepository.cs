using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using DataStorage.DAL.Interfaces;
using DataStorage.DAL.Entities;
using System;

namespace DataStorage.DAL.Repositories
{
    public class UsersRepository : IUsersRepository
    {
        private readonly UserManager<UserEntity> _userManager;
        private readonly SignInManager<UserEntity> _signInManager;

        public UsersRepository(UserManager<UserEntity> userManager, SignInManager<UserEntity> signInManager)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
        }

        public async Task<IdentityResult> ConfirmEmailAsync(UserEntity user, string token)
        {
            return await _userManager.ConfirmEmailAsync(user, token);
        }

        public async Task<IdentityResult> CreateUserAsync(string userEmail, string userPassword)
        {
            UserEntity user = new UserEntity { Email = userEmail, UserName = userEmail };
            var result = await _userManager.CreateAsync(user, userPassword);

            return result;
        }

        public async Task<string> GetEmailTokenAsync(UserEntity user)
        {
            return await _userManager.GenerateEmailConfirmationTokenAsync(user);
        }

        public async Task<UserEntity> GetUserByIdAsync(string userId)
        {
            return await _userManager.FindByIdAsync(userId);
        }

        public async Task<UserEntity> GetUserByNameAsync(string userEmail)
        {
            return await _userManager.FindByEmailAsync(userEmail);
        }

        public async Task<bool> IsEmailConfirmedAsync(UserEntity user)
        {
            return await _userManager.IsEmailConfirmedAsync(user);
        }

        public async Task<SignInResult> SignInUserAsync(string userEmail, string userPassword, bool rememberMe)
        {
            return await _signInManager.PasswordSignInAsync(userEmail, userPassword, rememberMe, false);
        }

        /*public async void LogOut()
        {
            await _signInManager.SignOutAsync();
        }*/
    }
}