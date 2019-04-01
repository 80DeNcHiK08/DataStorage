using System.Threading.Tasks;
using DataStorage.DAL.Entities;
using Microsoft.AspNetCore.Identity;

namespace DataStorage.DAL.Interfaces
{
    public interface IUsersRepository
    {
        Task<SignInResult> GetUserAsync(string userEmail, string userPassword, bool rememberMe);
        Task<IdentityResult> CreateUserAsync(string userEmail, string userPassword);
        //void LogOut();
    }
}