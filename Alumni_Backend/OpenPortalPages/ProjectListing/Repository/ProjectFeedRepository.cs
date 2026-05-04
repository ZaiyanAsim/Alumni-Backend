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
                

            if (query.SeekingSponsors == true)
                q = q.Where(p => p.Is_Sponsorship_Available==true && p.Is_Sponsored==false);

            if (query.SeekingMentors == true)
                q = q.Where(p => p.Is_Mentorship_Available==true && p.Is_Mentored==false);
           
            if (query.ProjectTypeIds is { Count: > 0 })
            {
                var projectTypeIds = _context.Projects
                    .Where(i => query.ProjectTypeIds.Contains(i.Project_Type_ID!.Value))
                    .Select(i => i.Project_ID);

                q = q.Where(p => projectTypeIds.Contains(p.Project_ID));
            }
            
            if (query.ProjectIndustryIds is { Count: > 0 })
            {
                var projectIdsInIndustry = _context.Project_Industry
                    .Where(i => query.ProjectIndustryIds.Contains(i.Project_Industry_Parameter_ID!.Value))
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
                    int cursorPriority = (cursorProject.Is_Sponsored == true ? 1 : 0)
                                       + (cursorProject.Is_Mentored == true ? 1 : 0);

                    q = q.Where(p =>
                        ((p.Is_Sponsored == true ? 1 : 0) + (p.Is_Mentored == true ? 1 : 0)) < cursorPriority ||
                        (((p.Is_Sponsored == true ? 1 : 0) + (p.Is_Mentored == true ? 1 : 0)) == cursorPriority
                            && p.Project_Year < query.CursorYear.Value) ||
                        (((p.Is_Sponsored == true ? 1 : 0) + (p.Is_Mentored == true ? 1 : 0)) == cursorPriority
                            && p.Project_Year == query.CursorYear.Value
                            && p.Project_ID < query.CursorProjectId.Value)
                    );
                }
            }

            var raw = await q
                .OrderByDescending(p => (p.Is_Sponsored == true ? 1 : 0) + (p.Is_Mentored == true ? 1 : 0))
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
