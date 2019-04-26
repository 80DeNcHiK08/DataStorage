using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using DataStorage.DAL.Entities;
using Microsoft.AspNetCore.Http;

namespace DataStorage.DAL
{
    public class ApplicationContext : IdentityDbContext<UserEntity>
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
            Database.EnsureDeleted();
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<DocumentEntity>().HasKey(de => de.DocumentId);

            builder.Entity<UserDocument>().HasKey(ud => new { ud.UserId, ud.DocumentId });

            builder.Entity<UserDocument>()
                .HasOne<UserEntity>(ud => ud.User)
                .WithMany(ue => ue.UserDocuments)
                .HasForeignKey(ud => ud.UserId);

            builder.Entity<UserDocument>()
                .HasOne<DocumentEntity>(ud => ud.Document)
                .WithMany(de => de.UserDocuments)
                .HasForeignKey(ud => ud.DocumentId);

            base.OnModelCreating(builder);
        }

        public DbSet<DocumentEntity> Documents { get; set; }
    }
}
