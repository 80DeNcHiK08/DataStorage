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
using Microsoft.AspNetCore.Http;

namespace Configuration
{
    public static class WebServiceExtentions
    {
        public static IServiceCollection AddWebServices(this IServiceCollection services)
        {
            services.AddDbContext<ApplicationContext>(options =>
               options.UseSqlServer("server=mssqlserver.c78p8wykbefv.eu-central-1.rds.amazonaws.com;database=DataStorage;uid=root;pwd=rootroot123;",
                    x => x.MigrationsAssembly("DataStorage.DAL")));

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
            services.TryAddScoped<IUsersService, UsersService>();
            services.TryAddScoped<IDocumentRepository, DocumentRepository>();
            services.TryAddScoped<IDocumentService, DocumentService>();
            services.TryAddScoped<IPathProvider, PathProvider>();
            services.TryAddScoped<IEmailService, EmailService>();
            services.TryAddScoped<IUserDocumentRepository, UserDocumentRepository>();
            services.TryAddScoped<ISharingService, SharingService>();

            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddTransient<IPathProvider, PathProvider>();

            return services;
        }
    }
}
