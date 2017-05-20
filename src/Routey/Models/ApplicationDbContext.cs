using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Routey.Models
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public virtual DbSet<Location> Locations { get; set; }
        public virtual DbSet<Route> Routes { get; set; }
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlServer(@"Data Source=tcp:routeydbserver.database.windows.net,1433;Initial Catalog=Routey_db;User Id=berausch@routeydbserver;Password=haPPy631");
        }

        public ApplicationDbContext()
        {

        }


    }
}
