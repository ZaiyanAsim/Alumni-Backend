

using Alumni_Portal.Infrastrcuture.Data_Models;
using Alumni_Portal.Infrastructure.Data_Models;
using Microsoft.EntityFrameworkCore;
using Project.Infrastructure.Data_Models;
using Shared.TenantService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Alumni_Portal.Infrastructure.Persistence
{
    public class ProjectDbContext : DbContext
    {
        private readonly ITenantService _tenantService;
        public ProjectDbContext(DbContextOptions<ProjectDbContext> options, ITenantService tenantService) : base(options)
        {
            _tenantService = tenantService;
        }

        public DbSet<Projects> Projects { get; set; }
        public DbSet<Project_Industry> Project_Industry { get; set; }

        public DbSet<Project_Individuals> Project_Individuals { get; set; }

        public DbSet<Project_Tech_Stack> Project_Tech_Stack { get; set; }

        public DbSet<Project_Attachments> Project_Attachments { get; set; }

        public DbSet <Project_Results> Project_Results { get; set; }

        public DbSet<Project_Delivarables> Project_Deliverables { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Projects>(entity => {

                entity.HasQueryFilter(p => p.Client_ID == 1 && p.Campus_ID == 1);

                entity.HasMany(e => e.Project_Industry)
              .WithOne(e => e.Project)
              .HasForeignKey(e => e.Project_ID)
              .OnDelete(DeleteBehavior.Cascade);
            }

            );

            modelBuilder.Entity<Project_Individuals>()
            .HasOne(pi => pi.Project)
            .WithMany(p => p.Project_Individuals)
             .HasForeignKey(pi => pi.Project_ID);

            


        }

    }
}
