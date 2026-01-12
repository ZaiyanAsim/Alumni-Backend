using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;
using Shared.TenantService;
using Users.Infrastructure.Data_Models;

namespace Users.Infrastructure.Persistance
{
    internal class AppDbContext : DbContext
    {
        private readonly ITenantService _tenantService;
        public AppDbContext(
            DbContextOptions<AppDbContext> options,
            ITenantService tenantService
        ) : base(options)
        {
            _tenantService = tenantService;
        }

        public DbSet<Individuals> Individuals { get; set; }
        public DbSet<Individual_Academics> Individual_Academics { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Individuals>().HasQueryFilter(i =>
                i.Client_ID == 1 && i.Campus_ID == 1
            );
            modelBuilder.Entity<Individual_Academics>()
                 .HasOne<Individuals>()
                 .WithMany(i => i.Academics)
                 .HasForeignKey(ia => ia.Individual_ID)
                 .HasPrincipalKey(i => i.Individual_ID)
                 .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
