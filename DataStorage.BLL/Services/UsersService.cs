using DataStorage.BLL.Interfaces;
using DataStorage.DAL.Interfaces;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Identity;

namespace DataStorage.BLL.Services
{
    public class UserService : IUsersService
    {
        public IUsersRepository _usersRepo { get; }

        public UserService(IUsersRepository usersRepo)
        {
            _usersRepo = usersRepo ?? throw new ArgumentNullException(nameof(usersRepo));
        }

        public async Task<SignInResult> GetUserAsync(string userEmail, string userPassword, bool rememberMe)
        {
            var user = await _usersRepo.GetUserAsync(userEmail, userPassword, rememberMe);
            return user;
        }

        public async Task<IdentityResult> CreateUserAsync(string userEmail, string userPassword)
        {
            return await _usersRepo.CreateUserAsync(userEmail, userPassword);
        }

        /*public void LogOut()
        {
            _usersRepo.LogOut();
        }*/
    }
}