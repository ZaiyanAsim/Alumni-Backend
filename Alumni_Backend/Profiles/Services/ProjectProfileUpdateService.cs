using Alumni_Portal.FileUploads;
using Alumni_Portal.FileUploads.DTO;
using Alumni_Portal.Infrastructure.Data_Models;
using Alumni_Portal.Profiles.DTO;


public class ProjectProfileUpdateService
{
    private readonly AttachmentService _attachmentService;
    private readonly ProjectUpdateRepo _repo;
    private readonly FileService _fileService;
   

    public ProjectProfileUpdateService(AttachmentService attachmentService, ProjectUpdateRepo repo, FileService fileService)
    {
        _attachmentService = attachmentService;
        _repo = repo;
        _fileService = fileService;
    }


    public async Task<string> AddProjectAttachment(int projectId, DocumentUploadRequestDTO newAttachment)
    {
        var response = await _attachmentService.UploadDocument(newAttachment);

        if (!string.IsNullOrEmpty(response.ErrorMessage))
            return response.ErrorMessage;

        var attachment = new Project_Attachments
        {
            Project_ID = projectId,
            Attachment_Title = response.File.Attachment_Title,
            Attachment_Description = response.File.Attachment_Description,
            Attachment_File_Name = response.File.Attachment_File_Name,
            Attachment_File_Location = response.File.Attachment_File_Location,
            Attachment_Size = response.File.Attachment_Size,
            Attachment_Date = DateTime.UtcNow,
        };

        await _repo.AddProjectAttachmentAsync(attachment);
        return "Attachment added successfully.";
    }


    
    //private async Task<UploadResponseDTO> UploadProjectMediaAsync(MediaUploadRequestDTO mediaData)
    //{
    //    return await _fileService.UploadMedia(mediaData);
    //}

   
    public async Task<List<FileUploadDTO>>  AddProjectMediaAsync(int projectId, List<IFormFile> media)
    {

        var response = await _fileService.UploadMedia(media);

        if (!string.IsNullOrEmpty(response.errorMessage) || response.errors.Any())
        {
            throw new Exception(response.errorMessage ?? string.Join(", ", response.errors));
        }

        var mediaRecords = response.UploadedFiles.Select(f => new Project_Media
        {
            Project_ID = projectId,
            Media_Date = f.Media_Date,
            Media_Title = f.Media_Title,
            Media_File_Name = f.Media_File_Name,
            Media_File_Location = f.Media_File_Location,
        }).ToList();


        await _repo.AddProjectMediaAsync(mediaRecords);

        return response.UploadedFiles;
    }

    public async Task DeleteProjectAttachment(int attachmentId) =>
        await _repo.DeleteProjectAttachmentAsync(attachmentId);

    // ── Results ───────────────────────────────────────────────────────────────

    public async Task AddProjectResult(int projectId, ProjectResultsDTO dto)


    {

        try
        {
            if (dto.Image != null)
            {
                var files = new List<IFormFile> { dto.Image };

                var response = await AddProjectMediaAsync(projectId, files);



                dto.Image_Url = response[0].Media_File_Location;


            }

            await _repo.AddProjectResultAsync(projectId, dto);
        }

        catch (Exception ex)
        {
            throw;
        }
        }














    public async Task DeleteProjectResult(int resultId) =>
        await _repo.DeleteProjectResultAsync(resultId);


    public async Task AddProjectDeliverable(int projectId, ProjectDeliverablesDTO dto) =>
        await _repo.AddProjectDeliverableAsync(projectId, dto);

    
    public async Task DeleteProjectDeliverable(int deliverableId) =>
        await _repo.DeleteProjectDeliverableAsync(deliverableId);
}


