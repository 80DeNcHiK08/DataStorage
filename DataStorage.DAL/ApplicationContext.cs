using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

using DataStorage.DAL.Entities;

namespace DataStorage.DAL
{
    public class ApplicationContext : IdentityDbContext<ApplicationUser>
    {
        //public ApplicationContext(string connectionString) : base(connectionString) { }
        
        public DbSet<ClientProfile> ClientProfiles { get; set; }
    }
}
