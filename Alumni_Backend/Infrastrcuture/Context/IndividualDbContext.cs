using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;
using Shared.TenantService;
using Alumni_Portal.Infrastructure.Data_Models;

namespace Alumni_Portal.Infrastructure.Persistance
{
    public class IndividualDbContext : DbContext
    {
        private readonly ITenantService _tenantService;
        public IndividualDbContext(
            DbContextOptions<IndividualDbContext> options,
            ITenantService tenantService
        ) : base(options)
        {
            _tenantService = tenantService;
        }

        public DbSet<Individuals> Individuals { get; set; }
        public DbSet<Individual_Academics> Individual_Academics { get; set; }

        public DbSet<Individual_Work_Experience> Individual_Work_Experience { get; set; }   

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Individuals>().HasQueryFilter(i =>
                i.Client_ID == 1 && i.Campus_ID == 1
            );
            modelBuilder.Entity<Individual_Academics>()
                 .HasOne<Individuals>()
                 .WithMany(i => i.Academic_Details)
                 .HasForeignKey(ia => ia.Individual_ID)
                 .HasPrincipalKey(i => i.Individual_ID)
                 .OnDelete(DeleteBehavior.Cascade);
            
            modelBuilder.Entity<Individual_Work_Experience>()
                 .HasOne<Individuals>()
                 .WithMany()
                 .HasForeignKey(iwe => iwe.Individual_ID)
                 .HasPrincipalKey(i => i.Individual_ID)
                 .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
