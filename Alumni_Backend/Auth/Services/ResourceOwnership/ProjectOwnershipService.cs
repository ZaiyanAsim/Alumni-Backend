using Alumni_Portal.Infrastructure.Persistance;
using Alumni_Portal.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
namespace Alumni_Portal.Auth.Services.ResourceOwnership
{
    public class ProjectOwnershipService
    {
        private ProjectDbContext _context;
        private IndividualDbContext _individualContext;
        public ProjectOwnershipService(ProjectDbContext context)
        {
            _context = context;
        }
        
        public async Task<bool> IsOwnerAsync(int projectId, int individualId)
        {
            return await _context.Projects
                .Where(p => p.Project_ID == projectId)
                .Join(
                    _context.Project_Individuals,
                    project => project.Project_ID,
                    projectIndividual => projectIndividual.Project_ID,
                    (project, projectIndividual) => projectIndividual
                )
                .Join(
                    _individualContext.Individuals,
                    projectIndividual => projectIndividual.Individual_ID,
                    individual => individual.Individual_ID,
                    (projectIndividual, individual) => new { projectIndividual, individual }
                )
                .AnyAsync(x =>
                    x.projectIndividual.Individual_ID == individualId &&
                    x.individual.Individual_Is_Alumni == false);
        }



    }
}
