using Microsoft.EntityFrameworkCore;
using Alumni_Portal.TenantConfiguration.Data_Models;
namespace Alumni_Portal.TenantConfiguration
{
    public class ConfigurationDbContext : DbContext
    {
        public ConfigurationDbContext(DbContextOptions<ConfigurationDbContext> options) : base(options)
        {
        }
        public DbSet<Parameters> Parameters { get; set; } = null!;
        public DbSet<Parameter_Values> Parameter_Values { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Parameters>(entity =>
            {
                entity.HasKey(e => e.Parameter_ID);
                entity.HasQueryFilter(p => p.Client_ID == 1 );
            });
            modelBuilder.Entity<Parameter_Values>(entity =>
            {
                entity.HasKey(e => e.Parameter_Value_ID);
                entity.HasQueryFilter(p => p.Client_ID == 1 );
            });
        }
    }
}
