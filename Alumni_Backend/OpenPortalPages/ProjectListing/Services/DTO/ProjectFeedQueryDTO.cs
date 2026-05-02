namespace Alumni_Portal.OpenPortalPages.ProjectListing.Services.DTO
{
    public class ProjectFeedQueryDTO
    {
        public int? CursorYear { get; set; }
        public int? CursorProjectId { get; set; }
        public int PageSize { get; set; } = 20;

        public List<int>? ProjectTypeIds { get; set; }
        public List<int>? ProjectIndustryIds { get; set; }

        public bool? SeekingMentors { get; set; } 
        public bool? SeekingSponsors { get; set; } 
    }
}
