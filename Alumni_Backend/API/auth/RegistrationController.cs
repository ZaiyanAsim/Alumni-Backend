using Alumni_Portal.Auth.Services.DTO;
using Entity_Directories.Services;
using Entity_Directories.Services.DTO;
using Microsoft.AspNetCore.Mvc;

[ApiController]
public class RegistrationController : ControllerBase
{
    private readonly RegistrationRequestService _service;

    public RegistrationController(RegistrationRequestService service) => _service = service;


    [HttpPost("api/Auth/registration-request")]
    public async Task<IActionResult> Submit([FromBody] SubmitRegistrationRequestDTO dto)
    {
        var requestId = await _service.SubmitAsync(dto);

        var userType = dto.UserType.ToLower();
        var needsOtp = userType is "student" or "supervisor";

        return Ok(new
        {
            requestId,
            requiresOtp = needsOtp,
            message = needsOtp
                ? $"A verification code has been sent to {dto.Email}. Please enter it to complete your registration."
                : "Registration request submitted successfully. You will be notified once approved.",
        });
    }


    [HttpPost("api/Auth/registration-request/verify-otp")]
    public async Task<IActionResult> VerifyOtp([FromBody] VerifyRegistrationOtpDTO dto)
    {
        await _service.VerifyOtpAsync(dto);
        return Ok(new { message = "Email verified. Your account has been created — you may now log in." });
    }


    [HttpPost("api/Auth/registration-request/resend-otp")]
    public async Task<IActionResult> ResendOtp([FromBody] ResendRegistrationOtpDTO dto)
    {
        await _service.ResendOtpAsync(dto.RequestId);
        return Ok(new { message = "A new verification code has been sent." });
    }


    [HttpDelete("api/Auth/registration-request/{id}")]
    public async Task<IActionResult> Cancel(int id)
    {
        await _service.CancelAsync(id);
        return Ok(new { message = "Registration request cancelled." });
    }


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