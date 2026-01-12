
using Admin.Application.Handlers;
using Admin.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Users.Application.Handlers;
using Users.Application.DTO;

namespace Admin.Controllers

{

    //[Authorize(Roles = "Admin")]
    [Route("api/Admin/users")]
    [ApiController]

    public class UserController : Controller

    {
        //private userHandler _handler;
        private DirectoryHandler _userDirectory;
        public UserController( DirectoryHandler userDirectory)
        {
            //_handler = handler;
            _userDirectory = userDirectory;
        }




        
        // POST: AuthController/Create
        [HttpGet]

        public async Task<ActionResult> getUsers([FromQuery] string type, int _page, int _size)
        {
            
                var response = await _userDirectory.GetUsersPaginated(type, _page, _size);
                return Ok(response);
            
            
        }

        [HttpPost("create")]
        [Consumes("application/json")]

        public async Task<IActionResult> create([FromBody] NewUserDTO newUser, [FromQuery] string type)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                int createdId= await _userDirectory.CreateUser(newUser);
                var response = new 
                {
                    Status = "Success",
                    Message = "User successfully created.",
                    UserId = createdId
                };

                return Ok(response);
            }

            catch (Exception)
            {
                return StatusCode(500, new { message = "An error occurred while processing your request" });
            }
        }

        

    }

}

