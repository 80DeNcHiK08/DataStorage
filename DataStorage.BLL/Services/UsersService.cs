using DataStorage.BLL.Interfaces;
using DataStorage.DAL.Interfaces;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using DataStorage.BLL.DTOs;

namespace DataStorage.BLL.Services
{
    public class UserService : IUsersService
    {
        public IUsersRepository _usersRepo { get; }
        private readonly PathProvider _pProvider;

        public UserService(IUsersRepository usersRepo, PathProvider pProvider)
        {
            _usersRepo = usersRepo ?? throw new ArgumentNullException(nameof(usersRepo));
            _pProvider = pProvider;
        }

        public async Task<SignInResult> GetUserAsync(string userEmail, string userPassword, bool rememberMe)
        {
            var user = await _usersRepo.GetUserAsync(userEmail, userPassword, rememberMe);
            return user;
        }

        public async Task<IdentityResult> CreateUserAsync(string userEmail, string userPassword)
        {
            UserDTO user = new UserDTO {Email = userEmail, UserName = userEmail};
            await _pProvider.CreateFolderOnRegister(user.Id.ToString(), user);
            return await _usersRepo.CreateUserAsync(userEmail, userPassword);
        }

        public async Task LogOut()
        {
            await _usersRepo.LogOut();
        }
    }
}