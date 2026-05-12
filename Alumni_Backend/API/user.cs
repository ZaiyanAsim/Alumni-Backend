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
            userDirectoryDTO? user = int.TryParse(id, out int numericId)
                ? await _userDirectory.GetUserByNumericId(numericId)
                : await _userDirectory.GetUser(id);

            if (user == null)
                return NotFound(new { message = "No user found with this ID" });

            return Ok(new { data = user });
        }


       
        [HttpGet]

        public async Task<ActionResult> getUsers([FromQuery] string type, int _page, int _size)
        {
            
                var response = await _userDirectory.GetUsersPaginated(type, _page, _size);
                
                 return Ok(response);
            
            
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(string id, [FromBody] UpdateUserDTO dto)
        {
            userDirectoryDTO? existing = int.TryParse(id, out int numericId)
                ? await _userDirectory.GetUserByNumericId(numericId)
                : await _userDirectory.GetUser(id);

            if (existing == null)
                return NotFound(new { message = "No user found with this ID" });

            await _userDirectory.UpdateUserAsync(existing.Individual_ID, dto);
            return Ok();
        }

        [HttpGet("{userId}/work-experience")]
        public async Task<IActionResult> GetWorkExperience(int userId)
        {
            var list = await _userDirectory.GetWorkExperienceAsync(userId);
            return Ok(new { data = list });
        }

        [HttpPost("{userId}/work-experience")]
        public async Task<IActionResult> AddWorkExperience(int userId, [FromBody] AddWorkExperienceDTO dto)
        {
            var newId = await _userDirectory.AddWorkExperienceAsync(userId, dto);
            return Ok(new { data = newId });
        }

        [HttpDelete("{userId}/work-experience/{workExpId}")]
        public async Task<IActionResult> DeleteWorkExperience(int userId, int workExpId)
        {
            await _userDirectory.DeleteWorkExperienceAsync(workExpId);
            return Ok();
        }

        [HttpPost("{userId}/academics")]
        public async Task<IActionResult> AddAcademic(int userId, [FromBody] AddAcademicDTO dto)
        {
            var newId = await _userDirectory.AddAcademicAsync(userId, dto);
            return Ok(new { data = newId });
        }

        [HttpPatch("{userId}/academics/{academicId}")]
        public async Task<IActionResult> UpdateAcademic(int userId, int academicId, [FromBody] UpdateAcademicDTO dto)
        {
            await _userDirectory.UpdateAcademicAsync(academicId, dto);
            return Ok();
        }

        [HttpDelete("{userId}/academics/{academicId}")]
        public async Task<IActionResult> DeleteAcademic(int userId, int academicId)
        {
            await _userDirectory.DeleteAcademicAsync(academicId);
            return Ok();
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

