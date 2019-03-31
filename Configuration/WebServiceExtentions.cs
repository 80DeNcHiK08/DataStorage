using DataStorage.DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.DependencyInjection;
using DataStorage.BLL.Contracts;
using DataStorage.BLL;
using DataStorage.DAL.Contracts;

namespace Configuration
{
    public static class WebServiceExtentions
    {
        public static IServiceCollection AddWebServices(this IServiceCollection services)
        {
            services.AddDbContext<ApplicationContext>(options =>
               options.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=DataStorage;Trusted_Connection=True;"));

            services.TryAddScoped<IUsersRepository, UsersRepository>();
            services.TryAddScoped<IUsersService, UserService>();

            return services;
        }
    }
}
