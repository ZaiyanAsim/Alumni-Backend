using Alumni_Portal.Infrastructure.Persistance;
using Alumni_Portal.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
namespace Alumni_Portal.Auth.Services.ResourceOwnership
{
    public class ProjectOwnershipService
    {
        private readonly ProjectDbContext _context;

        public ProjectOwnershipService(ProjectDbContext context)
        {
            _context = context;
        }

        public async Task<bool> IsOwnerAsync(int projectId, int individualId)
        {
            return await _context.Project_Individuals
                .AnyAsync(pi =>
                    pi.Project_ID == projectId &&
                    pi.Individual_ID == individualId);
        }
    }
}
