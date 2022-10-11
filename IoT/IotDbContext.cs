using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IoT.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace IoT
{
    public class IotDbContext : DbContext
    {

        public IotDbContext(DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<Collection> Collections { get; set; }
        public DbSet<ApiKey> ApiKeys { get; set; }
        public DbSet<Item> Items { get; set; }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseMySql(PCDataDLL.SecureData.DataDictionary["IoTDB"]);
        //}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(IotDbContext).Assembly);
        }
    }
}
