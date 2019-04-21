using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using DataStorage.DAL.Interfaces;
using DataStorage.DAL.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using System;
using Microsoft.AspNetCore.Authentication;

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
            return await _userManager.ConfirmEmailAsync(user, token);;
        }

        public async Task<IdentityResult> CreateUserAsync(string userEmail, string userPassword)
        {
            UserEntity user = new UserEntity { Email = userEmail, UserName = userEmail, StorageSize = 1073741824, RemainingStorageSize = 1073741824,
                FirstName = "First Name", LastName = "Last Name" };

            return await _userManager.CreateAsync(user, userPassword);;
        }

        public async Task<IdentityResult> CreateUserAsync(string userEmail)
        {
            
            UserEntity user = new UserEntity { Email = userEmail, UserName = userEmail, StorageSize = 1073741824, RemainingStorageSize = 1073741824,
                FirstName = "First Name", LastName = "Last Name" };

            return await _userManager.CreateAsync(user);;
        }

        public void DeleteUserAsync(string userId)
        {
            _context.Users.Remove(_context.Users.Find(userId));
            _context.SaveChanges();
        }

        public async Task<string> GetEmailTokenAsync(UserEntity user)
        {      
            return await _userManager.GenerateEmailConfirmationTokenAsync(user);;
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

        public async Task SignInUserAsync(UserEntity user, bool isPersistent)
        {
            await _signInManager.SignInAsync(user, isPersistent);
        }

        public async Task<ExternalLoginInfo> GetExternalLoginInfoAsync()
        {
           return await _signInManager.GetExternalLoginInfoAsync();
        }

        public async Task<SignInResult> ExternalLoginSignInAsync(string loginProvider, string providerKey, bool isPersistent, bool bypassTwoFactor)
        {
            return await _signInManager.ExternalLoginSignInAsync(loginProvider, providerKey, isPersistent, bypassTwoFactor);
        }

        public Task<IdentityResult> AddLoginAsync(UserEntity user, ExternalLoginInfo loginInfo)
        {
            return _userManager.AddLoginAsync(user, loginInfo);
        }

        public AuthenticationProperties ConfigureExternalAuthenticationProperties(string provider, string redirectUrl)
        {
            return _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
        }

        public async Task LogOut()
        {
            await _signInManager.SignOutAsync();
        }

        public string GetUserId(ClaimsPrincipal user)
        {
            return _userManager.GetUserId(user);
        }

        public async Task<string> GetResetPasswordTokenAsync(UserEntity user)
        {
            return await _userManager.GeneratePasswordResetTokenAsync(user);
        }

        public async Task<IdentityResult> ResetPasswordAsync(UserEntity user, string token, string newPassword)
        {
            return await _userManager.ResetPasswordAsync(user, token, newPassword);
        }

        public async Task<bool> ConfirmIncrease(UserEntity user)
        {
            if(user.StorageSize < 2147483648)
                user.RemainingStorageSize += 1073741824;
            user.StorageSize = 2147483648;
            await _context.SaveChangesAsync();
            return true; ;
        }

        public async Task<bool> NameChange(string UserId, string UserFirstName, string UserLastName)
        {
            var user = await GetUserByIdAsync(UserId);
            user.FirstName = UserFirstName;
            user.LastName = UserLastName;
            await _context.SaveChangesAsync();
            return true;
        }

        /*public async Task<IdentityResult> ConfirmEmailAsync(UserEntity user, string token)
        {
            return await _userManager.ConfirmEmailAsync(user, token);;
        }*/
    }
}