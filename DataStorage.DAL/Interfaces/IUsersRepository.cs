using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using DataStorage.DAL.Entities;
using Microsoft.AspNetCore.Authentication;

namespace DataStorage.DAL.Interfaces
{
    public interface IUsersRepository
    {
        Task<SignInResult> SignInUserAsync(string userEmail, string userPassword, bool rememberMe);
        Task SignInUserAsync(UserEntity user, bool isPersistent);
        Task<IdentityResult> CreateUserAsync(string userEmail, string userPassword);
        Task<IdentityResult> CreateUserAsync(string userEmail);
        Task<IdentityResult> ConfirmEmailAsync(UserEntity user, string token);
        Task<UserEntity> GetUserByNameAsync(string userEmail);
        Task<UserEntity> GetUserByIdAsync(string userId);
        Task<string> GetEmailTokenAsync(UserEntity user);
        Task<bool> IsEmailConfirmedAsync(UserEntity user);
        Task<ExternalLoginInfo> GetExternalLoginInfoAsync();
        Task<SignInResult> ExternalLoginSignInAsync(string loginProvider, string providerKey, bool isPersistent, bool bypassTwoFactor);
        Task<IdentityResult> AddLoginAsync(UserEntity user, ExternalLoginInfo loginInfo);
        AuthenticationProperties ConfigureExternalAuthenticationProperties(string provider, string redirectUrl);
        Task<string> GetResetPasswordTokenAsync(UserEntity user);
        Task<IdentityResult> ResetPasswordAsync(UserEntity user, string token, string newPassword);
        Task LogOut();
        string GetUserId(ClaimsPrincipal user);
    }
}