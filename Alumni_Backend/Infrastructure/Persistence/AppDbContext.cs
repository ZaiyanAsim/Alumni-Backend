using Admin.Infrastructure.Data_Models;
using Microsoft.EntityFrameworkCore;
using Shared.TenantService;

namespace Admin.Infrastructure.Data
{
    public class AppDbContext : DbContext
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

        public DbSet<Projects>Projects { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Individuals>().HasQueryFilter(i =>
                i.Client_ID == 1 && i.Campus_ID==1
            );
            modelBuilder.Entity<Individual_Academics>()
                 .HasOne<Individuals>()                          
                 .WithMany(i => i.Academics)                     
                 .HasForeignKey(ia => ia.Individual_ID)          
                 .HasPrincipalKey(i => i.Individual_ID)          
                 .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Projects>().HasQueryFilter(P =>
            P.Client_ID == 1 && P.Campus_ID == 1);
            
        }

    }
    

}




