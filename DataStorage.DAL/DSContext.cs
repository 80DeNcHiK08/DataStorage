using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using DataStorage.DAL.Models;

namespace DataStorage.DAL
{
    class DSContext : DbContext
    {
        public DSContext(DbContextOptions<DSContext> options)
            : base(options)
        { }

        DbSet<UserEntity> Users { get; set; }
    }
}
