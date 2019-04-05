using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace DataStorage.BLL.Interfaces
{
    public interface IUsersService
    {
        Task<SignInResult> GetUserAsync(string userEmail, string userPassword, bool rememberMe);
        Task<IdentityResult> CreateUserAsync(string userEmail, string userPassword);
        Task LogOut();
    }
}