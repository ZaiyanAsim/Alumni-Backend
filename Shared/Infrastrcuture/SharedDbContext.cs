using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;
using Shared.Data_Model;
using Shared.DTO;
using Shared.TenantService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Infrastrcuture
{
    public class SharedDbContext : DbContext
    {
        private ITenantService _tenantService;
        public SharedDbContext (
            DbContextOptions<SharedDbContext>options, ITenantService tenantService)
        {

            _tenantService = tenantService;
        }

        public DbSet<Parameters> Parameters;
        public DbSet<Parameter_Values> Parameter_Values;
        public DbSet<Project_Individuals_Map> Project_Individuals_Map;


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Parameters>().HasQueryFilter(i =>
              i.Client_ID == 1
            );

            modelBuilder.Entity<Parameter_Values>().HasQueryFilter(i =>
             i.Client_ID == 1
            );
                
        }

            
       

        
    }
}
