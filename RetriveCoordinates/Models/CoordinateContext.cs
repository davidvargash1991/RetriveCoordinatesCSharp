using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace RetriveCoordinates.Models
{
    public class CoordinateContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=.;Database=efCoreTest;Integrated Security=true");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Countries>()
                .Property(c => c.CountryId)
                .UseSqlServerIdentityColumn();

            modelBuilder.Entity<Countries>()
                .HasMany(e => e.Cities)
                .WithOne(e => e.Country);

            modelBuilder.Entity<Cities>()
                .Property(c => c.CityId)
                .UseSqlServerIdentityColumn();
        }

        public DbSet<Cities> Cities { get; set; }

        public DbSet<Cities> Countries { get; set; }
    }
}
