using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

using DataStorage.DAL.Entities;

namespace DataStorage.DAL
{
    public class ApplicationContext : IdentityDbContext<UserEntity>
    {
        public DbSet<DocumentEntity> Documents { get; set; }
        public DbSet<UserDocument> UserDocuments { get; set; }

        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            :base(options)
        {
            Database.EnsureCreated();
        }
    }
}
