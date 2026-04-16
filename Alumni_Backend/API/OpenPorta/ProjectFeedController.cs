using Alumni_Portal.OpenPortalPages.MainPage.Services;
using Alumni_Portal.OpenPortalPages.MainPage.Services.DTO;
using Alumni_Portal.OpenPortalPages.ProjectListing.Services;
using Alumni_Portal.OpenPortalPages.ProjectListing.Services.DTO;
using Entity_Directories.Services;
using Microsoft.AspNetCore.Mvc;
namespace Alumni_Portal.API.OpenPortal
{

    [Route("api/Admin/projects")]
    [ApiController]
    public class ProjectFeedController : Controller
    {
        private readonly ProjectFeedService _feedService;

        public ProjectFeedController(ProjectFeedService feedService)
        {
            _feedService = feedService;
        }

        [HttpGet("feed")]
        
        public async Task<IActionResult> GetFeed([FromQuery] ProjectFeedQueryDTO query, CancellationToken ct)
        {
            if (query.PageSize < 1 || query.PageSize > 100)
                return BadRequest("PageSize must be between 1 and 100.");

            var result = await _feedService.GetProjectFeedAsync(query, ct);

            if (!result.Projects.Any())
                return NoContent();

            return Ok(result);
        }

    }
}
