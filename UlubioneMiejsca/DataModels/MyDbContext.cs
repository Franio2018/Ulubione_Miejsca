using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace UlubioneMiejsca.DataModels
{
    public class MyDbContext:DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Location> Locations { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=.;Database=UlubioneMiejsce;Trusted_Connection=True;TrustServerCertificate=True");
        }
    }
}
