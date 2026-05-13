using Alumni_Portal.Infrastructure.Persistance;
using Alumni_Portal.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Alumni_Portal.OpenPortalPages.Stats
{
    public class PortalStatsRepository
    {
        private readonly IndividualDbContext _individualContext;
        private readonly ProjectDbContext _projectContext;

        public PortalStatsRepository(IndividualDbContext individualContext, ProjectDbContext projectContext)
        {
            _individualContext = individualContext;
            _projectContext = projectContext;
        }

        public async Task<PortalStatsDTO> GetStatsAsync(CancellationToken ct = default)
        {
            var alumniCount = await _individualContext.Individuals
                .AsNoTracking()
                .CountAsync(i => i.Individual_Is_Alumni == true, ct);

            var projectCount = await _projectContext.Projects
                .AsNoTracking()
                .CountAsync(ct);

            var industryCount = await _projectContext.Project_Industry
                .AsNoTracking()
                .Select(i => i.Project_Industry_Value)
                .Distinct()
                .CountAsync(ct);

            return new PortalStatsDTO
            {
                AlumniCount   = alumniCount,
                ProjectCount  = projectCount,
                IndustryCount = industryCount,
            };
        }
    }

    public class PortalStatsDTO
    {
        public int AlumniCount   { get; init; }
        public int ProjectCount  { get; init; }
        public int IndustryCount { get; init; }
    }
}
