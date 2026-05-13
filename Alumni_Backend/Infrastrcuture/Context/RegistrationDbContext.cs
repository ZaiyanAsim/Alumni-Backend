using Alumni_Portal.Infrastructure.Data_Models;
using Microsoft.EntityFrameworkCore;

namespace Alumni_Portal.Infrastructure.Persistance
{
    public class RegistrationDbContext : DbContext
    {
        public RegistrationDbContext(DbContextOptions<RegistrationDbContext> options)
            : base(options) { }

        public DbSet<Registration_Request> Registration_Requests { get; set; }
    }
}
