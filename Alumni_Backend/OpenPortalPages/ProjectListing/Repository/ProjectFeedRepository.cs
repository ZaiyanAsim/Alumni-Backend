using Alumni_Portal.Infrastructure.Persistance;
using Alumni_Portal.Infrastructure.Persistence;
using Alumni_Portal.OpenPortalPages.ProjectListing.Services.DTO;
using Alumni_Portal.Profiles.DTO;
using Microsoft.EntityFrameworkCore;

namespace Alumni_Portal.OpenPortalPages.ProjectListing.Repository
{
    public class ProjectFeedRepository
    {
        public readonly ProjectDbContext _context;
        private readonly IndividualDbContext _individualContext;

        public ProjectFeedRepository(ProjectDbContext context, IndividualDbContext individualContext)
        {
            _context = context;
            _individualContext = individualContext;
        }



        public async Task<(List<MetaDataDTO> Projects, bool HasMore)> GetProjectFeedAsync(
    ProjectFeedQueryDTO query,
    CancellationToken ct = default)
        {
            int fetchCount = query.PageSize + 1;

            var q = _context.Projects
                .AsNoTracking();
                

            if (query.SeekingSponsors == true)
            {
                var sponsoredProjectIds = _context.Project_Individuals
                    .Where(pi => pi.Individual_Role.ToLower() == "sponsor")
                    .Select(pi => pi.Project_ID);

                q = q.Where(p => p.Is_Sponsorship_Available == true && !sponsoredProjectIds.Contains(p.Project_ID));
            }

            if (query.SeekingMentors == true)
            {
                var mentoredProjectIds = _context.Project_Individuals
                    .Where(pi => pi.Individual_Role.ToLower() == "mentor")
                    .Select(pi => pi.Project_ID);

                q = q.Where(p => p.Is_Mentorship_Available == true && !mentoredProjectIds.Contains(p.Project_ID));
            }

            if (!string.IsNullOrWhiteSpace(query.Search))
            {
                var s = query.Search.Trim().ToLower();

                // Resolve matching individual IDs first (separate DbContext — can't cross in one query)
                var matchingIndividualIds = await _individualContext.Individuals
                    .AsNoTracking()
                    .Where(i => i.Individual_Name.ToLower().Contains(s))
                    .Select(i => i.Individual_ID)
                    .ToListAsync(ct);

                var projectIdsWithMatchingMember = await _context.Project_Individuals
                    .AsNoTracking()
                    .Where(pi => matchingIndividualIds.Contains(pi.Individual_ID))
                    .Select(pi => pi.Project_ID)
                    .Distinct()
                    .ToListAsync(ct);

                q = q.Where(p =>
                    p.Project_Name.ToLower().Contains(s) ||
                    (p.Project_Description != null && p.Project_Description.ToLower().Contains(s)) ||
                    (p.Project_Academic_ID != null && p.Project_Academic_ID.ToLower().Contains(s)) ||
                    (p.Project_Type_Value != null && p.Project_Type_Value.ToLower().Contains(s)) ||
                    (p.Project_Industries != null && p.Project_Industries.ToLower().Contains(s)) ||
                    p.Project_Year.ToString().Contains(s) ||
                    projectIdsWithMatchingMember.Contains(p.Project_ID)
                );
            }
           
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

            var intermediate = await q
                .OrderByDescending(p => (p.Is_Sponsored ? 1 : 0) + (p.Is_Mentored ? 1 : 0))
                .ThenByDescending(p => p.Project_Year)
                .ThenByDescending(p => p.Project_ID)
                .Take(fetchCount)
                .Select(p => new
                {
                    p.Project_ID,
                    p.Logo_Url,
                    p.Project_Academic_ID,
                    p.Project_Name,
                    p.Project_Type_Value,
                    p.Video_Url,
                    p.Project_Year,
                    p.Project_Industries,
                    p.Project_Description,
                    p.Is_Mentored,
                    p.Is_Sponsored,
                    p.Is_Mentorship_Available,
                    p.Is_Sponsorship_Available,
                })
                .ToListAsync(ct);

            bool hasMore = intermediate.Count == fetchCount;
            var page = intermediate.Take(query.PageSize).ToList();

            var projectIds = page.Select(p => p.Project_ID).ToList();

            var techStackByProject = await _context.Project_Tech_Stack
                .AsNoTracking()
                .Where(t => projectIds.Contains(t.Project_ID))
                .Select(t => new { t.Project_ID, t.Technology_Value })
                .ToListAsync(ct);

            var memberIdsByProject = await _context.Project_Individuals
                .AsNoTracking()
                .Where(pi => projectIds.Contains(pi.Project_ID))
                .Select(pi => new { pi.Project_ID, pi.Individual_ID, pi.Individual_Role })
                .ToListAsync(ct);

            var allIndividualIds = memberIdsByProject.Select(m => m.Individual_ID).Distinct().ToList();
            var individualNames = allIndividualIds.Count > 0
                ? await _individualContext.Individuals
                    .AsNoTracking()
                    .Where(i => allIndividualIds.Contains(i.Individual_ID))
                    .Select(i => new { i.Individual_ID, i.Individual_Name, i.Individual_Institution_ID })
                    .ToDictionaryAsync(
                        i => i.Individual_ID,
                        i => $"{i.Individual_Name} ({i.Individual_Institution_ID})",
                        ct)
                : new Dictionary<int, string>();

            var raw = page.Select(p => new MetaDataDTO
            {
                Project_ID = p.Project_ID,
                Logo_Url = p.Logo_Url,
                Project_Academic_ID = p.Project_Academic_ID,
                Project_Name = p.Project_Name,
                Project_Type = p.Project_Type_Value,
                Video_Url = p.Video_Url,
                Project_Year = p.Project_Year,
                Project_Category = p.Project_Industries,
                Project_Description = p.Project_Description,
                Is_Mentored = memberIdsByProject.Any(m => m.Project_ID == p.Project_ID && m.Individual_Role.Equals("Mentor", StringComparison.OrdinalIgnoreCase)),
                Is_Sponsored = memberIdsByProject.Any(m => m.Project_ID == p.Project_ID && m.Individual_Role.Equals("Sponsor", StringComparison.OrdinalIgnoreCase)),
                Is_Mentorship_Available = p.Is_Mentorship_Available,
                Is_Sponsorship_Available = p.Is_Sponsorship_Available,
                Tech_Stack = techStackByProject
                    .Where(t => t.Project_ID == p.Project_ID)
                    .Select(t => t.Technology_Value)
                    .Take(5)
                    .ToList(),
                Members = memberIdsByProject
                    .Where(m => m.Project_ID == p.Project_ID)
                    .Select(m => individualNames.TryGetValue(m.Individual_ID, out var name) ? name : null)
                    .Where(n => n != null)
                    .ToList()!,
            }).ToList();

            return (raw, hasMore);
        }
    }
}
