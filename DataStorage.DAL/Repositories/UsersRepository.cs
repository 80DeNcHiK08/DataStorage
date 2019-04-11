using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using DataStorage.DAL.Interfaces;
using DataStorage.DAL.Entities;
using System;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace DataStorage.DAL.Repositories
{
    public class UsersRepository : IUsersRepository
    {
        private readonly UserManager<UserEntity> _userManager;
        private readonly SignInManager<UserEntity> _signInManager;
        private readonly ApplicationContext _context;

        public UsersRepository(UserManager<UserEntity> userManager, SignInManager<UserEntity> signInManager, ApplicationContext context)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IdentityResult> ConfirmEmailAsync(UserEntity user, string token)
        {
            // // attempt to untrack instance of UserEntity
            // var user = await _userManager.FindByIdAsync(userId);
            // _context.Entry(user).State = EntityState.Detached;
            // _context.SaveChanges();
            var result = await _userManager.ConfirmEmailAsync(user, token);

            return result;
        }


        // alternative way
        // public async Task<IdentityResult> ConfirmEmailAsync(string userId, string token)
        // {
        //     var user = await _userManager.FindByIdAsync(userId);
        //     _context.Entry(user).State = EntityState.Detached;
        //     _context.SaveChanges();
            
        //     return await _userManager.ConfirmEmailAsync(user, token);
        // }

        public async Task<IdentityResult> CreateUserAsync(string userEmail, string userPassword)
        {
            UserEntity user = new UserEntity { Email = userEmail, UserName = userEmail };
            var result = await _userManager.CreateAsync(user, userPassword);
            // _context.Entry(user).State = EntityState.Detached;
            // ClientProfile client = new ClientProfile { Id = user.Id, Name = user.Email };
            // _context.ClientProfiles.Add(client);
            // _context.SaveChanges();

            return result;
        }

        public async Task<string> GetEmailTokenAsync(UserEntity user)
        {
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            _context.Entry(user).State = EntityState.Detached;
            
            return token;
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
            var result = await _userManager.IsEmailConfirmedAsync(user);
            _context.Entry(user).State = EntityState.Detached;

            return result;
        }

        public async Task<SignInResult> SignInUserAsync(string userEmail, string userPassword, bool rememberMe)
        {
            return await _signInManager.PasswordSignInAsync(userEmail, userPassword, rememberMe, false);
        }

        public async Task SignInUserAsync(UserEntity user, bool isPersistent)
        {
            await _signInManager.SignInAsync(user, isPersistent);
            _context.Entry(user).State = EntityState.Detached;
        }

        public async void LogOut()
        {
            await _signInManager.SignOutAsync();
        }
    }
}