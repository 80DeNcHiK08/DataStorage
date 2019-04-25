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
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            // builder.Entity<UserDocument>().HasKey(ud => new { ud.GuestEmail, ud.DocumentId });
            builder.Entity<DocumentEntity>().HasKey(de => de.DocumentId);
            // builder.Entity<DocumentEntity>().Property(de => de.DocumentId).IsRequired();

            // builder.Entity<UserEntity>()
            //     .HasMany(ue => ue.Documents)
            //     .WithOne(de => de.Owner)
            //     .HasForeignKey(de => de.OwnerId);
            //     .OnDelete(DeleteBehavior.Cascade);

            // builder.Entity<UserDocument>().Property(ud => ud.DocumentId).IsRequired();
            // builder.Entity<UserDocument>().Property(ud => ud.UserId).IsRequired();
            builder.Entity<UserDocument>().HasKey(ud => new { ud.UserId, ud.DocumentId });

            builder.Entity<UserDocument>()
                .HasOne<UserEntity>(ud => ud.User)
                .WithMany(ue => ue.UserDocuments)
                .HasForeignKey(ud => ud.UserId);

            builder.Entity<UserDocument>()
                .HasOne<DocumentEntity>(ud => ud.Document)
                .WithMany(de => de.UserDocuments)
                .HasForeignKey(ud => ud.UserId);

            base.OnModelCreating(builder);
        }

        public DbSet<DocumentEntity> Documents { get; set; }
        // public DbSet<UserDocument> UserDocuments { get; set; }
    }
}
