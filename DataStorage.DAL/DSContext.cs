using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

using DataStorage.DAL.Models;

namespace DataStorage.DAL
{
    public class DSContext : DbContext
    {
        public DSContext(DbContextOptions<DSContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        DbSet<UserEntity> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<UserEntity>().HasKey(u => new { u.Email, u.Username });
        }
    }
}
