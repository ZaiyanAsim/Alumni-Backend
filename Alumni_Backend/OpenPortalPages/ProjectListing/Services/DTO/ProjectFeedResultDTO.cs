using Alumni_Portal.Profiles.DTO;

namespace Alumni_Portal.OpenPortalPages.ProjectListing.Services.DTO
{
    public class ProjectFeedResultDTO
    {
        public List<MetaDataDTO> Projects { get; set; } = [];
        public int? NextCursorYear { get; set; }
        public int? NextCursorProjectId { get; set; }
        public bool HasMore => NextCursorYear is not null;
    }
}
