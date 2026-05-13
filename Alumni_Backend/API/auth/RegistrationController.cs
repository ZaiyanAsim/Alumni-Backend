using Entity_Directories.Services;
using Entity_Directories.Services.DTO;
using Microsoft.AspNetCore.Mvc;

namespace Alumni_Portal.API.Auth
{
    [ApiController]
    public class RegistrationController : ControllerBase
    {
        private readonly RegistrationRequestService _service;

        public RegistrationController(RegistrationRequestService service)
        {
            _service = service;
        }

        // Public — no auth required
        [HttpPost("api/Auth/registration-request")]
        public async Task<IActionResult> Submit([FromBody] SubmitRegistrationRequestDTO dto)
        {
            await _service.SubmitAsync(dto);
            return Ok(new { message = "Registration request submitted successfully. You will be notified once approved." });
        }

        // Admin endpoints
        [HttpGet("api/Admin/registration-requests")]
        public async Task<IActionResult> GetPending()
        {
            var requests = await _service.GetPendingAsync();
            return Ok(new { data = requests });
        }

        [HttpPost("api/Admin/registration-requests/{id}/approve")]
        public async Task<IActionResult> Approve(int id)
        {
            await _service.ApproveAsync(id);
            return Ok(new { message = "Request approved and user created." });
        }

        [HttpPost("api/Admin/registration-requests/{id}/reject")]
        public async Task<IActionResult> Reject(int id)
        {
            await _service.RejectAsync(id);
            return Ok(new { message = "Request rejected." });
        }
    }
}