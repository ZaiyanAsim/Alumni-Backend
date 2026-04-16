using Microsoft.AspNetCore.Mvc;

using Entity_Directories.Services.DTO;
using Alumni_Portal.Infrastructure.Data_Models;
using Entity_Directories.Services;



namespace Admin.Controllers

{

    //[Authorize(Roles = "Admin")]
    [Route("api/Admin/users")]
    [ApiController]

    public class UserController : Controller

    {

        private UserService _userDirectory;
        public UserController( UserService userDirectory)
        {
            _userDirectory = userDirectory;
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(string id)
        {
            var user = await _userDirectory.GetUser(id);
            if (user == null)
            {
                return NotFound(new {message="No user found with this Institution ID"});
            }

            var response = new
            {
                data = user
            };


            return Ok(response);
        }


       
        [HttpGet]

        public async Task<ActionResult> getUsers([FromQuery] string type, int _page, int _size)
        {
            
                var response = await _userDirectory.GetUsersPaginated(type, _page, _size);
                
                 return Ok(response);
            
            
        }


        [HttpPost("create")]
        [Consumes("application/json")]

        public async Task<IActionResult> create([FromBody] NewUserDTO newUser)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            
                 await _userDirectory.CreateUser(newUser);
                var response = new 
                {
                    Status = "Success",
                    Message = "User successfully created.",
                    
                    accessUrl=$"/api/Admin/users/{newUser.Institution_ID}"
                };

                return Ok(response);
            

            
        }


        [HttpDelete("delete")]

        public async Task<IActionResult> DeleteUsers([FromBody] List<int> individualIds)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            Console.WriteLine(individualIds == null ? "IDS IS NULL" : $"IDS COUNT: {individualIds.Count}");
            if (individualIds == null)
            {
                return BadRequest(new{  message="No ids to delete"});
            }
            List<int> failedDeletes = await _userDirectory.DeleteUsersBulk(individualIds);

            var response = new
            {

                failedDeletes = failedDeletes
            };


            return Ok(response);




        }



    }

}

