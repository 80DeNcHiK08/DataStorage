using System.Threading.Tasks;
using DataStorage.BLL.Models;

namespace DataStorage.BLL.Contracts
{
    public interface IUsersService
    {
        Task<User> GetUserAsync(string userEmail, string userPassword);
        Task<User> CreateUserAsync(string userEmail, string userPassword);
    }
}