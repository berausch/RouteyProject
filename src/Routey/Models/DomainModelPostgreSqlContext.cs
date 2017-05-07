using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Routey.Models
{
    public class DomainModelPostgreSqlContext : DbContext
    {
        public DbSet<Location> Locations { get; set; }
        public DbSet<Route> Routes { get; set; }
        public DomainModelPostgreSqlContext(DbContextOptions<DomainModelPostgreSqlContext> options) : base(options)
        {
        }
         protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Location>().HasKey(m => m.LocationId);
            builder.Entity<Route>().HasKey(m => m.RouteId);
 
            // shadow properties
            base.OnModelCreating(builder);
        }

        public override int SaveChanges()
        {
            ChangeTracker.DetectChanges();

            updateUpdatedProperty<Route>();
            updateUpdatedProperty<Location>();

            return base.SaveChanges();
        }

        private void updateUpdatedProperty<T>() where T : class
        {
            var modifiedSourceInfo =
                ChangeTracker.Entries<T>()
                    .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);

 
        }
        public DomainModelPostgreSqlContext()
        {

        }


    }
}
