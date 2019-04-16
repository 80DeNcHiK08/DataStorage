using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

using DataStorage.DAL.Entities;
using Microsoft.AspNetCore.Http;

namespace DataStorage.DAL
{
    public class ApplicationContext : IdentityDbContext<UserEntity>
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            :base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<DocumentEntity> Documents { get; set; }
        public DbSet<DocumentEntity> UserDocuments { get; set; }
    }
}
