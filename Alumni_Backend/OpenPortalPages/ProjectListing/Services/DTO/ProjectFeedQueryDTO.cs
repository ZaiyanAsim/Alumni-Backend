namespace Alumni_Portal.OpenPortalPages.ProjectListing.Services.DTO
{
    public class ProjectFeedQueryDTO
    {
        public int? CursorYear { get; set; }
        public int? CursorProjectId { get; set; }
        public int PageSize { get; set; } = 20;

        public List<int>? IndustryParameterIds { get; set; }  

        public bool? AvailableForSponsorship { get; set; }
        public bool? AvailableForMentorship { get; set; }
    }
}
