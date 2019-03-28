using DataStorage.DAL.Entities;
using System;

namespace DataStorage.DAL.Interfaces
{
    public interface IClientManager : IDisposable
    {
        void Create(ClientProfile item);
    }
}
