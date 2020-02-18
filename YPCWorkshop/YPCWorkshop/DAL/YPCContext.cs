
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using YPCWorkshop.Models;

namespace YPCWorkshop.DAL
{
    public class YPCContext : DbContext
    {
        public DbSet<Tool> Tools { get; set; }
        public DbSet<Volunteer> Volunteers { get; set; }
        public DbSet<Rental> Rentals { get; set; } 

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
        
    }
}