using DataStorage.DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.DependencyInjection;
using DataStorage.BLL.Interfaces;
using DataStorage.BLL.Services;
using DataStorage.DAL.Interfaces;
using DataStorage.DAL.Repositories;
using DataStorage.DAL.Entities;
using Microsoft.AspNetCore.Identity;

namespace Configuration
{
    public static class WebServiceExtentions
    {
        public static IServiceCollection AddWebServices(this IServiceCollection services)
        {
            services.AddDbContext<ApplicationContext>(options =>
               options.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=DataStorage;Trusted_Connection=True;"));

            services.AddIdentity<UserEntity, IdentityRole>(options =>
                {
                    options.Password.RequireNonAlphanumeric = false;
                })
                .AddEntityFrameworkStores<ApplicationContext>()
                .AddDefaultTokenProviders();

            services.AddAuthentication()
                .AddGoogle(googleOptions =>
                {
                    googleOptions.ClientId = "683865802300-uqhcsp84nqbqjmmsr1r8v98dh8c9mmme.apps.googleusercontent.com";
                    googleOptions.ClientSecret = "XOxXjisXAkPFOoIDPUs1T_KJ";
                });

            services.TryAddScoped<IUsersRepository, UsersRepository>();
            services.TryAddScoped<IUsersService, UserService>();
            services.TryAddScoped<IEmailService, EmailService>();

            return services;
        }
    }
}
