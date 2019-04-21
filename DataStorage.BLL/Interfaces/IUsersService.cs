using System.Security.Claims;
using System.Threading.Tasks;
using DataStorage.BLL.DTOs;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;

namespace DataStorage.BLL.Interfaces
{
    public interface IUsersService
    {
        Task<SignInResult> SignInUserAsync(string userEmail, string userPassword, bool rememberMe);
        Task SignInUserAsync(UserDTO user, bool isPersistent);
        Task<IdentityResult> CreateUserAsync(string userEmail, string userPassword);
        Task<IdentityResult> CreateUserAsync(string userEmail);
        Task<IdentityResult> ConfirmEmailAsync(string userId, string token);
        Task<UserDTO> GetUserByNameAsync(string userEmail);
        Task<UserDTO> GetUserByIdAsync(string userId);
        Task<string> GetEmailTokenAsync(UserDTO user);
        Task<bool> IsEmailConfirmedAsync(UserDTO user);
        Task<ExternalLoginInfo> GetExternalLoginInfoAsync();
        Task<SignInResult> ExternalLoginSignInAsync(string loginProvider, string providerKey, bool isPersistent, bool bypassTwoFactor);
        Task<IdentityResult> AddLoginAsync(string userEmail, ExternalLoginInfo loginInfo);
        AuthenticationProperties ConfigureExternalAuthenticationProperties(string provider, string redirectUrl);
        Task<string> GetResetPasswordTokenAsync(UserDTO user);
        Task<IdentityResult> ResetPasswordAsync(string userEmail, string token, string newPassword);
        Task LogOut();
        string GetUserId(ClaimsPrincipal user);
        Task<bool> ConfirmIncreaseAsync(string userId);
        Task<bool> NameChange(string UserId, string UserFirstName, string UserLastName);
        void DeleteUserAsync(string userId);
    }
}