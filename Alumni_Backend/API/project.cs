
using Admin.Application.Handlers;
using Admin.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Project.Application.Handlers;
using Project.Application.DTO;

namespace Admin.Controllers

{

    
    [Route("api/Admin/projects")]
    [ApiController]

    public class ProjectController : Controller

    {
        private DirectoryHandler _handler;


        public ProjectController(DirectoryHandler handler)
        {
            _handler = handler;
        }





        // POST: AuthController/Create
        [HttpGet]

        public async Task<ActionResult> GetProjects([FromQuery] ProjectFilters filters , int _page, int _size)
        {
            try
            {
                var response = await _handler.GetProjectsPaginated(filters, _page, _size);
                return Ok(response);
            }
            catch (Exception)
            {


                return StatusCode(500, new { message = "A database error occurred while processing your request" });
            }
        }

        



    }

}

