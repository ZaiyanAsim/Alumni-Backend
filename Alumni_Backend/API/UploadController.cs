using Alumni_Portal.FileUploads;
using Microsoft.AspNetCore.Mvc;

namespace Alumni_Portal.API
{
    [ApiController]
    [Route("api/media")]
    public class UploadController : ControllerBase
    {
        private readonly FileService _fileService;

        public UploadController(FileService fileService)
        {
            _fileService = fileService;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadMedia([FromForm] List<IFormFile> Media)
        {
            var response = await _fileService.UploadMedia(Media);

            if (!response.UploadedFiles.Any())
                return BadRequest(new { message = response.errorMessage, errors = response.errors });

            if (response.errors.Any())
                return StatusCode(207, new { uploaded = response.UploadedFiles, errors = response.errors });

            return Ok(response.UploadedFiles);
        }
    }
}