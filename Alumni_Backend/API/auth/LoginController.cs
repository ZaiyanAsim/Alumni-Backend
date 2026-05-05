using Alumni_Portal.Auth.Services.DTO;
using Alumni_Portal.Auth.Services;

using Microsoft.AspNetCore.Mvc;

namespace Alumni_Portal.API.Auth
{
    [ApiController]
    [Route("api/Auth")]
    public class LoginController : ControllerBase
    {
        private readonly LoginMainService _loginService;

        public LoginController(LoginMainService loginService)
        {
            _loginService = loginService;
        }

        /// <summary>
        /// Authenticates an alumni user and returns a signed JWT.Now this is inte
        /// POST /api/auth/login
        /// </summary>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            LoginResult result = await _loginService.LoginAsync(request);

            if (!result.Success)
                return Unauthorized(new { message = result.ErrorMessage });

            return Ok(result.Response);
        }

        [HttpPost("enter-dummy-passwords")]
        public async Task<IActionResult> EnterDummyPasswords(string password, string id)
        {
                await _loginService.EnterDummyPasswords(password,id);
                return Ok(new { message = "Dummy passwords entered successfully." });
        }
    }
}