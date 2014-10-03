using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

using SportsStore.Domain.Entities;

namespace SportsStore.Domain.Concrete
{
    public class EFDbContext : DbContext
    {
        //// ----------------------------------------------------------------------------------------------------------

        public EFDbContext() : base("EFDbContext.ConnectionString")
        {
        }

        //// ----------------------------------------------------------------------------------------------------------
		 
        public DbSet<Product> Products { get; set; }

        //// ----------------------------------------------------------------------------------------------------------

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }

        //// ----------------------------------------------------------------------------------------------------------
    }
}
