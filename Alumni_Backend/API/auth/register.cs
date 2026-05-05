//using Alumni_Portal.API.Auth.DTOs;
//using Alumni_Portal.API.Auth.Services;
//using Microsoft.AspNetCore.Mvc;

//namespace Alumni_Portal.API.Auth
//{
//    [ApiController]
//    [Route("api/auth")]
//    public class RegisterController : ControllerBase
//    {
//        private readonly IRegisterService _registerService;

//        public RegisterController(IRegisterService registerService)
//        {
//            _registerService = registerService;
//        }

//        /// <summary>
//        /// Registers a new alumni user.
//        /// POST /api/auth/register
//        /// </summary>
//        [HttpPost("register")]
//        public async Task<IActionResult> Register([FromBody] RegisterRequestDto request)
//        {
//            if (!ModelState.IsValid)
//                return BadRequest(ModelState);

//            var result = await _registerService.RegisterAsync(request);

//            if (!result.Success)
//                return Conflict(new { message = result.ErrorMessage });

//            return CreatedAtAction(nameof(Register), new { message = "Registration successful.", userId = result.UserId });
//        }
//    }
//}