using Alumni_Portal.Infrastrcuture.Data_Models;
using Alumni_Portal.Infrastructure.Data_Models;
using Entity_Directories.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;
using Shared.TenantService;

namespace Alumni_Portal.Infrastructure.Persistance
{
    public class SharedDbContext : DbContext
    {
        private readonly ITenantService _tenantService;
        public SharedDbContext(
            DbContextOptions<SharedDbContext> options,
            ITenantService tenantService
        ) : base(options)
        {
            _tenantService = tenantService;
        }
        public DbSet<Requests> Requests { get; set; }
        public DbSet<Events> Events { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Requests>().HasQueryFilter(i =>
                i.Client_ID == 1 && i.Campus_ID == 1
            );



        }
    }
}
