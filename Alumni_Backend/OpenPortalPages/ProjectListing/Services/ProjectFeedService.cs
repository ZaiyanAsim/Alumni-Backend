using Alumni_Portal.OpenPortalPages.ProjectListing.Repository;
using Alumni_Portal.OpenPortalPages.ProjectListing.Services.DTO;
using Alumni_Portal.Profiles.DTO;

namespace Alumni_Portal.OpenPortalPages.ProjectListing.Services
{
    public class ProjectFeedService
    {
        private readonly ProjectFeedRepository _repo;

        public ProjectFeedService(ProjectFeedRepository repo)
        {
            _repo = repo;
        }

        public async Task<ProjectFeedResultDTO> GetProjectFeedAsync(
            ProjectFeedQueryDTO query,
            CancellationToken ct = default)
        {
            int pageSize = Math.Clamp(query.PageSize, 1, 100);
            query.PageSize = pageSize;

            var (projects, hasMore) = await _repo.GetProjectFeedAsync(query, ct);

            MetaDataDTO? last = hasMore ? projects[^1] : null;

            return new ProjectFeedResultDTO
            {
                Projects = projects,
                NextCursorYear = last?.Project_Year,
                NextCursorProjectId = last?.Project_ID,
            };
        }
    }
}
