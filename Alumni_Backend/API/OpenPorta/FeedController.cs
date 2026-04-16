using Alumni_Portal.OpenPortalPages.MainPage.Services;
using Alumni_Portal.OpenPortalPages.MainPage.Services.DTO;
using Entity_Directories.Services;
using Microsoft.AspNetCore.Mvc;
namespace Alumni_Portal.API.OpenPortal
{

    [Route("api/Admin/posts")]
    [ApiController]
    public class FeedController : Controller
    {
        private readonly FeedService _feedService;

        public FeedController(FeedService feedService)
        {
            _feedService = feedService;
        }

        [HttpGet("feed")]
        public async Task<IActionResult> GetFeed([FromQuery] PostFeedQueryDTO query, CancellationToken ct)
        {
            if (query.PageSize < 1 || query.PageSize > 100)
                return BadRequest("PageSize must be between 1 and 100.");

            var result = await _feedService.GetFeedAsync(query, ct);

            if (!result.Posts.Any())
                return NoContent();

            return Ok(result);
        }

    }
}
