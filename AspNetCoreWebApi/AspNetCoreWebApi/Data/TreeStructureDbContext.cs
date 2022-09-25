using AspNetCoreWebApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Xml.Linq;

namespace AspNetCoreWebApi.Data
{
    public class TreeStructureDbContext : DbContext
    {
        private IConfiguration Configuration;

        public DbSet<Node> Nodes { get; set; }

        public TreeStructureDbContext(IConfiguration _configuration)
        {
            Configuration = _configuration;
        }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string connectionString = Configuration.GetConnectionString("Default");
            optionsBuilder.UseSqlServer(connectionString);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Node>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasMany(e => e.Childrens)
                    ;
            });
            modelBuilder.Entity<Node>().HasData(
               new Node
               {
                   Id = 1,
                   Name = "example element 1a",
                   ParentId = null
               },
               new Node
               {
                   Id = 2,
                   Name = "example element 1b",
                   ParentId = null
               },
               new Node
               {
                   Id = 3,
                   Name = "example element 1c",
                   ParentId = null
               },
               new Node
               {
                   Id = 4,
                   Name = "example element 1d",
                   ParentId = null
               },
               new Node
               {
                   Id = 5,
                   Name = "example element 2a",
                   ParentId = 1
               },
               new Node
               {
                   Id = 6,
                   Name = "example element 2b",
                   ParentId = 2

               },
               new Node
               {
                   Id = 7,
                   Name = "example element 2c",
                   ParentId = 3
               },
               new Node
               {
                   Id = 8,
                   Name = "example element 2d",
                   ParentId = 4
               },
               new Node
               {
                   Id = 9,
                   Name = "example element 3a",
                   ParentId = 5
               },
               new Node
               {
                   Id = 10,
                   Name = "example element 3b",
                   ParentId = 5
               },
               new Node
               {
                   Id = 11,
                   Name = "example element 3c",
                   ParentId = 6
               },
               new Node
               {
                   Id = 12,
                   Name = "example element 3d",
                   ParentId = 7
               }
               );
        }
    }
}
