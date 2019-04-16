using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace DataStorage.DAL.Interfaces
{
    public interface IUsersRepository
    {
        
        Task<SignInResult> GetUserAsync(string userEmail, string userPassword, bool rememberMe);
        Task<IdentityResult> CreateUserAsync(string userEmail, string userPassword);
        Task LogOut();
        string GetUserId(ClaimsPrincipal user);
    }
}