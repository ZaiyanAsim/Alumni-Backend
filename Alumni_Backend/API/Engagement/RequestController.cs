using Microsoft.AspNetCore.Mvc;
using Alumni_Portal.Engagement.Services;
using Alumni_Portal.Engagement.Services.DTO;
namespace Alumni_Portal.API.Engagement
{
    [Route("api/Engagement/requests")]
    [ApiController]
    public class RequestController : Controller
    {
        private readonly RequestProcessing _requestService;

        public RequestController(RequestProcessing requestService)
        {
            _requestService = requestService;
        }

        [HttpPost]
        public async Task<IActionResult> EngagementRequest(RequestDTO request )
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            await _requestService.ProcessRequest(request);
            return Ok();
        }


    }
}
