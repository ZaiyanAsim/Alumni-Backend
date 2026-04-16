using Alumni_Portal.Infrastructure.Persistence;
using Alumni_Portal.OpenPortalPages.ProjectListing.Services.DTO;
using Alumni_Portal.Profiles.DTO;
using Alumni_Portal.Profiles.Repositories.MappingExpressions;
using Microsoft.EntityFrameworkCore;

namespace Alumni_Portal.OpenPortalPages.ProjectListing.Repository
{
    public class ProjectFeedRepository
    {
        public readonly ProjectDbContext _context;

        public ProjectFeedRepository(ProjectDbContext context)
        {
            _context = context;
        }



        public async Task<(List<MetaDataDTO> Projects, bool HasMore)> GetProjectFeedAsync(
    ProjectFeedQueryDTO query,
    CancellationToken ct = default)
        {
            int fetchCount = query.PageSize + 1;

            var q = _context.Projects
                .AsNoTracking();
                

            if (query.AvailableForSponsorship == true)
                q = q.Where(p => p.Is_Sponsorship_Available);

            if (query.AvailableForMentorship == true)
                q = q.Where(p => p.Is_Mentorship_Available);

            if (query.IndustryParameterIds is { Count: > 0 })
            {
                var projectIdsInIndustry = _context.Project_Industry
                    .Where(i => query.IndustryParameterIds.Contains(i.Project_Industry_Parameter_ID!.Value))
                    .Select(i => i.Project_ID);

                q = q.Where(p => projectIdsInIndustry.Contains(p.Project_ID));
            }

            // ── Cursor condition ──────────────────────────────────────────────────────
            if (query.CursorYear.HasValue && query.CursorProjectId.HasValue)
            {
                var cursorProject = await _context.Projects
                    .AsNoTracking()
                    .Where(p => p.Project_ID == query.CursorProjectId.Value)
                    .Select(p => new { p.Is_Sponsored, p.Is_Mentored })
                    .FirstOrDefaultAsync(ct);

                if (cursorProject is not null)
                {
                    int cursorPriority = (cursorProject.Is_Sponsored ? 1 : 0)
                                       + (cursorProject.Is_Mentored ? 1 : 0);

                    q = q.Where(p =>
                        // Lower priority bucket
                        ((p.Is_Sponsored ? 1 : 0) + (p.Is_Mentored ? 1 : 0)) < cursorPriority ||
                        // Same priority, older year
                        (((p.Is_Sponsored ? 1 : 0) + (p.Is_Mentored ? 1 : 0)) == cursorPriority
                            && p.Project_Year < query.CursorYear.Value) ||
                        // Same priority, same year, lower ID
                        (((p.Is_Sponsored ? 1 : 0) + (p.Is_Mentored ? 1 : 0)) == cursorPriority
                            && p.Project_Year == query.CursorYear.Value
                            && p.Project_ID < query.CursorProjectId.Value)
                    );
                }
            }

            var raw = await q
                .OrderByDescending(p => (p.Is_Sponsored ? 1 : 0) + (p.Is_Mentored ? 1 : 0))
                .ThenByDescending(p => p.Project_Year)
                .ThenByDescending(p => p.Project_ID)
                .Take(fetchCount)
                .Select(ProjectProfileMapping.ToMetaDataDTO)
                .ToListAsync(ct);

            bool hasMore = raw.Count == fetchCount;
            return (raw.Take(query.PageSize).ToList(), hasMore);
        }
    }
}
