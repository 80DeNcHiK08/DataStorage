using System.Threading.Tasks;
using DataStorage.DAL.Entities;

namespace DataStorage.DAL.Contracts
{
    public interface IUsersRepository
    {
        Task<UserEntity> GetUserAsync(string userEmail, string userPassword);
        Task<UserEntity> CreateUserAsync(string userEmail, string userPassword);

    }
}