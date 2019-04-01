using System.Threading.Tasks;
using DataStorage.DAL.Entities;

namespace DataStorage.DAL.Interfaces
{
    public interface IUsersRepository
    {
        void GetUserAsync(string userEmail, string userPassword, bool rememberMe);
        void CreateUserAsync(string userEmail, string userPassword);
        void LogOut();
    }
}