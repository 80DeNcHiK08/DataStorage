using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using DataStorage.DAL.Entities;

namespace DataStorage.DAL.Interfaces
{
    public interface IUsersRepository
    {
        Task<SignInResult> SignInUserAsync(string userEmail, string userPassword, bool rememberMe);
        Task SignInUserAsync(UserEntity user, bool isPersistent);
        Task<IdentityResult> CreateUserAsync(string userEmail, string userPassword);
        Task<IdentityResult> ConfirmEmailAsync(UserEntity user, string token);
        // Task<IdentityResult> ConfirmEmailAsync(string userId, string token);
        Task<UserEntity> GetUserByNameAsync(string userEmail);
        Task<UserEntity> GetUserByIdAsync(string userId);
        Task<string> GetEmailTokenAsync(UserEntity user);
        Task<bool> IsEmailConfirmedAsync(UserEntity user);
        void LogOut();
    }
}