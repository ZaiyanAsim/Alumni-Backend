using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;
using Shared.TenantService;
using Alumni_Portal.Infrastructure.Data_Models;
using Entity_Directories.Repositories;

namespace Alumni_Portal.Infrastructure.Persistance
{
    public class PostDbContext : DbContext
    {
        private readonly ITenantService _tenantService;
        public PostDbContext(
            DbContextOptions<PostDbContext> options,
            ITenantService tenantService
        ) : base(options)
        {
            _tenantService = tenantService;
        }

        public DbSet<Posts> Posts { get; set; }
        public DbSet<Post_Mentions>Post_Mentions { get; set; }
        public DbSet<Post_Media> Post_Media { get; set; }
        public DbSet<Post_Attachment> Post_Attachment { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Posts>().HasQueryFilter(i =>
                i.Client_ID == 1 && i.Campus_ID == 1
            );

            

        }
    }
}
