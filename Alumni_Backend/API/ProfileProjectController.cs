using Microsoft.AspNetCore.Mvc;
using Alumni_Portal.Profiles.Services;

namespace Admin.Controllers
{
    [Route("api/Admin/profile-project")]
    [ApiController]
    public class ProfileProjectController : Controller
    {
        private readonly ProjectProfileService _service;

        public ProfileProjectController(ProjectProfileService service)
        {
            _service = service;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProjectProfile(int id, CancellationToken ct)
        {
            var profile = await _service.GetFullProfileAsync(id, ct);
            return Ok(new { data = profile });
        }
    }
}
