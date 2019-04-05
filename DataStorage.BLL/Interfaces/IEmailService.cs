using System.Threading.Tasks;
using DataStorage.BLL.DTOs;
using Microsoft.AspNetCore.Identity;

namespace DataStorage.BLL.Interfaces
{
    public interface IEmailService
    {
        Task SendEmailAsync(string email, string subject, string message);
    }
}