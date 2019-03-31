using System.Threading.Tasks;
using DataStorage.DAL.Contracts;
using DataStorage.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataStorage.DAL
{
    public class UsersRepository : IUsersRepository
    {
        ApplicationContext _context;

        public UsersRepository(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<UserEntity> CreateUserAsync(string userEmail, string userPassword)
        {
            var user = new UserEntity {Email = userEmail, Password = userPassword};
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            
            return user;
        }

        public async Task<UserEntity> GetUserAsync(string userEmail, string userPassword)
        {
            var user =  await _context.Users.FirstOrDefaultAsync(u => u.Email == userEmail && u.Password == userPassword);

            return user;
        }
    }
}