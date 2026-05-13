using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Entity_Directories.Services.DTO;
using Entity_Directories.Services;





namespace Admin.Controllers

{

    
    [Route("api/Admin/projects")]
    [ApiController]

    public class ProjectController : Controller

    {
        private ProjectService _handler;


        public ProjectController(ProjectService handler)
        {
            _handler = handler;
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetProject(string id)
        {
            if (id.IsNullOrEmpty())
            {
                return BadRequest(new { message = "Project Academic ID is null or empty" });
            }
            var project = await _handler.GetProject(id);

            var response = new
            {
                data = project
            };
            return Ok(response);
        }



        [HttpGet]

        public async Task<ActionResult> GetProjects([FromQuery] ProjectFilters filters , int _page, int _size)
        {
            
                var response = await _handler.GetProjectsPaginated(filters, _page, _size);
                return Ok(response);
            
            
        }

        [HttpPost("create")]
        public async Task<ActionResult> Create([FromBody] CreateProjectDTO newProject)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            int createdId = await _handler.CreateProject(newProject);
            
            var response = new
            {
                Status = "Success",
                Message = "Project successfully created.",
                Project_ID = createdId,
                accessUrl=$"/api/Admin/projects/{newProject.Project_Academic_ID}"
            };

            return Ok(response);


        }

        [HttpPost("create/members/{projectId}")]

        public async Task<IActionResult> AddProjectMembers(string projectId,List<ProjectIndividualDTO> members)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _handler.AddProjectMembers(projectId,members);

            var response = new
            {
                status = "Successful",
                message = $"Members added to Project {projectId}",
                projectId=projectId

            };


            return Ok(response);
        }



        [HttpDelete("delete")]

        public async Task<IActionResult> DeleteProjects([FromBody] List<int> projectIds)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            Console.WriteLine(projectIds == null ? "IDS IS NULL" : $"IDS COUNT: {projectIds.Count}");

            List<int> failedDeletes=await _handler.DeleteProjectsBulk(projectIds);

            var response = new {
              
                failedDeletes=failedDeletes
            };


            return Ok(response);
            
            


        }


    }

}

