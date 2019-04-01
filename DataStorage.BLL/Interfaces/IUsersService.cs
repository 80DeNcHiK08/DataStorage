using System.Threading.Tasks;
using DataStorage.BLL.DTOs;

namespace DataStorage.BLL.Interfaces
{
    public interface IUsersService
    {
        Task<UserDTO> GetUserAsync(string userEmail, string userPassword);
        Task<UserDTO> CreateUserAsync(string userEmail, string userPassword);
    }
}