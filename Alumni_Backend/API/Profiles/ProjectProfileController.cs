using Alumni_Portal.FileUploads;
using Alumni_Portal.FileUploads.DTO;
using Alumni_Portal.Infrastructure.Data_Models;
using Alumni_Portal.Profiles.DTO;
using Alumni_Portal.Profiles.Services;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata;

[Route("api/Admin/profile-project")]
[ApiController]
public class ProfileProjectController : ControllerBase
{
    private readonly ProjectProfileReadService _readService;
    private readonly ProjectProfileUpdateService _updateService;

    public ProfileProjectController(
        ProjectProfileReadService readService,
        ProjectProfileUpdateService updateService)
    {
        _readService = readService;
        _updateService = updateService;
    }



    [HttpGet("{id}")]
    public async Task<IActionResult> GetProjectProfile(int id, CancellationToken ct)
    {
        var profile = await _readService.GetFullProfileAsync(id, ct);
        return Ok(new { data = profile });
    }



    [HttpPost("{projectId}/attachments")]
    public async Task<IActionResult> AddProjectAttachment(
        int projectId,
        [FromBody] DocumentUploadRequestDTO document)
    {
        var response = await _updateService.AddProjectAttachment(projectId, document);
        return Ok(new { data = response });
    }

    //[HttpPost("{projectId}/media")]
    //public async Task<IActionResult> AddProjectMedia(
    //int projectId,
    //[FromBody] MediaUploadRequestDTO mediaData)
    //{
    //    if (mediaData.Files is null || mediaData.Files.Count == 0)
    //        return BadRequest("No media provided.");

    //    await _updateService.AddProjectMedia(projectId, mediaData);
    //    return Ok();
    //}
    //[HttpPost("{projectId}/media")]
    //public async Task<IActionResult> AddMediaToPost([FromForm] List<IFormFile> media, int projectId)
    //{
    //    var uploadResult = await _fileService.UploadMedia(media);

    //    if (!uploadResult.UploadedFiles.Any())
    //        return BadRequest(new { message = uploadResult.errorMessage, errors = uploadResult.errors });

    //    var postMediaList = uploadResult.UploadedFiles.Select(file => new Post_Media
    //    {
    //        Post_Id = postId,
    //        Media_Title = file.Media_Title,
    //        Media_Date = file.Media_Date,
    //        Media_File_Location = file.Media_File_Location,
    //        Media_File_Name = file.Media_File_Name,
    //        Progress_Value = "Uploaded",
    //        Status_Value = "Active",
    //        Progress_Id = 1,
    //        Status_Id = 1,
    //        Created_By_Id = 1,
    //        Created_By_Name = "Admin",
    //        Created_Date = DateTime.UtcNow,
    //    }).ToList();

    //    await _handler.AddMedia(postMediaList);

    //    return Ok(new { Status = "Success", message = "Media successfully associated with post." });
    //}

    [HttpDelete("attachments/{attachmentId}")]
    public async Task<IActionResult> DeleteProjectAttachment(int attachmentId)
    {
        await _updateService.DeleteProjectAttachment(attachmentId);
        return Ok();
    }

   

    [HttpPost("{projectId}/results")]
    public async Task<IActionResult> AddProjectResult(
        int projectId,
        [FromForm] ProjectResultsDTO dto)
    {
        await _updateService.AddProjectResult(projectId, dto);
        return Ok();
    }

    

    [HttpDelete("results/{resultId}")]
    public async Task<IActionResult> DeleteProjectResult(int resultId)
    {
        await _updateService.DeleteProjectResult(resultId);
        return Ok();
    }

  

    [HttpPost("{projectId}/deliverables")]
    public async Task<IActionResult> AddProjectDeliverable(
        int projectId,
        [FromBody] ProjectDeliverablesDTO dto)
    {
        await _updateService.AddProjectDeliverable(projectId, dto);
        return Ok();
    }

    

    [HttpDelete("deliverables/{deliverableId}")]
    public async Task<IActionResult> DeleteProjectDeliverable(int deliverableId)
    {
        await _updateService.DeleteProjectDeliverable(deliverableId);
        return Ok();
    }
}