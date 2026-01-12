using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Project.Infrastructure.Data_Models;
using Shared.TenantService;
namespace Project.Infrastructure.Persistence
{
    internal class AppDbContext : DbContext
    {
        private readonly ITenantService _tenantService;
        public AppDbContext(DbContextOptions<AppDbContext> options, ITenantService tenantService) : base(options)
        {
            _tenantService = tenantService;
        }

        public DbSet<Projects> Projects { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Projects>().HasQueryFilter(p =>
                p.Client_ID == 1 && p.Campus_ID == 1
            );
        }

    }
}
