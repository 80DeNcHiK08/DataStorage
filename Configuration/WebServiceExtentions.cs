using DataStorage.DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Configuration
{
    public static class WebServiceExtentions
    {
        public static IServiceCollection AddWebServices(this IServiceCollection services)
        {
            return services.AddDbContext<ApplicationContext>(options =>
               options.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=DataStorage;Trusted_Connection=True;"));
        }
    }
}
