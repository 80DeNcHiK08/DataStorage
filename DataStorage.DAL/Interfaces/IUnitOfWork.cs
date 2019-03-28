using System;
using System.Threading.Tasks;

using DataStorage.DAL.Identity;

namespace DataStorage.DAL.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        ApplicationUserManager UserManager { get; }
        IClientManager ClientManager { get; }
        Task SaveAsync();
    }
}
