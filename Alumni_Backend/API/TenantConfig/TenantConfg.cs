using Alumni_Portal.TenantConfiguration;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Admin.Controllers
{


    [Route("api/TenantConfig")]
    [ApiController]
    public class TenantConfg:ControllerBase
    {
        private ConfigService _configService;

        public TenantConfg(ConfigService configService)
        {
            _configService = configService;
        }
        //[HttpGet("Metadata")] //WO ajaye ga ismain sahi hai baaki ayega kahin aur 

        [HttpGet("Metadata/projects")]

        public async Task<ActionResult> GetProjectMetadata()
        {
            var metadata= await _configService.GetProjectMetadata();

            var response = new
            {
                status="Success",
                message="Project metadata fetched successfully",
                projectTypes = metadata.ProjectTypes,
                projectIndustries = metadata.ProjectIndustries

            };

            return Ok(response);
        }
        
        [HttpGet("Metadata/Individuals")]
        public async Task<ActionResult> GetIndividualAcademicMetadata()
        {
            var metadata= await _configService.GetIndividualMetadata();
            var response = new
            {
                status="Success",
                message="Individual metadata fetched successfully",
                
                programs=metadata.AcademicPrograms,
                departments=metadata.AcademicDepartments,
                designations=metadata.AcademicDesignations
            };
            return Ok(response);
        }

        [HttpGet("Metadata/projects/create")]
        public async Task<ActionResult> GetProjectCreateMetadata()
        {
            

            var metadata = await _configService.ProjectCreateMetadata();
            var response = new
            {
                status = "Success",
                message = "Metadata for Project Creation Successful",
                responseData = metadata
            };

            return Ok(response );

        }
    }
}
