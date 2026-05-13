using Alumni_Portal.OpenPortalPages.Stats;
using Microsoft.AspNetCore.Mvc;

namespace Alumni_Portal.API.OpenPortal
{
    [Route("api/portal/stats")]
    [ApiController]
    public class PortalStatsController : ControllerBase
    {
        private readonly PortalStatsRepository _repo;

        public PortalStatsController(PortalStatsRepository repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public async Task<IActionResult> GetStats(CancellationToken ct)
        {
            var stats = await _repo.GetStatsAsync(ct);
            return Ok(stats);
        }
    }
}
