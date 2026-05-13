using Alumni_Portal.Infrastrcuture.Data_Models;
using Microsoft.EntityFrameworkCore;

namespace Alumni_Portal.Infrastructure.Persistance
{
    public class EventDbContext : DbContext
    {
        public EventDbContext(DbContextOptions<EventDbContext> options) : base(options) { }

        public DbSet<Events> Events { get; set; }
    }
}
