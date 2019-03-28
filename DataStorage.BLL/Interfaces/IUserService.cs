using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

using DataStorage.BLL.DTOs;
using DataStorage.BLL.Infrastructure;

namespace DataStorage.BLL.Interfaces
{
    public interface IUserService
    {
        Task<OperationDetails> Create(UserDTO userDto);
        Task<ClaimsIdentity> Authenticate(UserDTO userDto);
    }
}
