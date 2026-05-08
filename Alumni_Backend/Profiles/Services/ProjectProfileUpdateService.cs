using Alumni_Portal.FileUploads;
using Alumni_Portal.FileUploads.DTO;
using Alumni_Portal.Infrastructure.Data_Models;
using Alumni_Portal.Profiles.DTO;
using Shared.Custom_Exceptions.ExceptionClasses;


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

    // ── Description ───────────────────────────────────────────────────────────

    public Task UpdateDescriptionAsync(int projectId, string description)
        => _repo.UpdateDescriptionAsync(projectId, description);

    // ── Members ───────────────────────────────────────────────────────────────

    public Task<int> AddMemberAsync(int projectId, int individualId, string role)
        => _repo.AddMemberAsync(projectId, individualId, role);

    public Task RemoveMemberAsync(int mapId)
        => _repo.RemoveMemberAsync(mapId);

    // ── Tech Stack ─────────────────────────────────────────────────────────────

    public Task<int> AddTechStackAsync(int projectId, string technologyValue, string? layerValue)
        => _repo.AddTechStackAsync(projectId, technologyValue, layerValue);

    public Task RemoveTechStackAsync(int stackId)
        => _repo.RemoveTechStackAsync(stackId);

    // ── Methodologies ─────────────────────────────────────────────────────────

    public Task<int> AddMethodologyAsync(int projectId, string methodologyValue)
        => _repo.AddMethodologyAsync(projectId, methodologyValue);

    public Task RemoveMethodologyAsync(int methodologyId)
        => _repo.RemoveMethodologyAsync(methodologyId);

    // ── Attachments ───────────────────────────────────────────────────────────

    public async Task<string> AddProjectAttachment(int projectId, DocumentUploadRequestDTO newAttachment)
    {
        var response = await _attachmentService.UploadDocument(newAttachment);

        if (!string.IsNullOrEmpty(response.ErrorMessage))
            throw new ValidationException(response.ErrorMessage);

        var attachment = new Project_Attachments
        {
            Project_ID = projectId,
            Attachment_Title = response.File.Attachment_Title,
            Attachment_Description = response.File.Attachment_Description,
            Attachment_File_Name = response.File.Attachment_File_Name,
            Attachment_File_Location = response.File.Attachment_File_Location,
            Attachment_Size = response.File.Attachment_Size,
            Attachment_Date = DateTime.UtcNow,
            Progress_ID = 1,
            Progress_Value = "Active",
            Status_ID = 1,
            Status_Value = "Active",
            Created_By_ID = 1,
            Created_By_Name = "Admin",
            Created_Date = DateTime.UtcNow,
        };

        await _repo.AddProjectAttachmentAsync(attachment);
        return response.File.Attachment_File_Location;
    }

    public async Task AddAttachmentLinkAsync(int projectId, string title, string url, string? description)
    {
        var attachment = new Project_Attachments
        {
            Project_ID = projectId,
            Attachment_Title = title,
            Attachment_Description = description,
            Attachment_File_Location = url,
            Attachment_File_Name = title,
            Attachment_Size = 0,
            Attachment_Date = DateTime.UtcNow,
            Progress_ID = 1,
            Progress_Value = "Active",
            Status_ID = 1,
            Status_Value = "Active",
            Created_By_ID = 1,
            Created_By_Name = "Admin",
            Created_Date = DateTime.UtcNow,
        };
        await _repo.AddProjectAttachmentAsync(attachment);
    }

    public Task DeleteProjectAttachment(int attachmentId)
        => _repo.DeleteProjectAttachmentAsync(attachmentId);

    // ── Results ───────────────────────────────────────────────────────────────

    public async Task AddProjectResult(int projectId, ProjectResultsDTO dto)
    {
        if (dto.Image != null)
        {
            var response = await _fileService.UploadMedia(new List<IFormFile> { dto.Image });
            dto.Image_Url = response.UploadedFiles.FirstOrDefault()?.Media_File_Location;
        }
        await _repo.AddProjectResultAsync(projectId, dto);
    }

    public async Task UpdateProjectResult(int projectId, int resultId, ProjectResultsDTO dto)
    {
        if (dto.Image != null)
        {
            var response = await _fileService.UploadMedia(new List<IFormFile> { dto.Image });
            dto.Image_Url = response.UploadedFiles.FirstOrDefault()?.Media_File_Location;
        }
        await _repo.UpdateProjectResultAsync(resultId, dto);
    }

    public Task DeleteProjectResult(int resultId)
        => _repo.DeleteProjectResultAsync(resultId);

    // ── Deliverables ──────────────────────────────────────────────────────────

    public Task AddProjectDeliverable(int projectId, ProjectDeliverablesDTO dto)
        => _repo.AddProjectDeliverableAsync(projectId, dto);

    public Task UpdateProjectDeliverable(int projectId, int deliverableId, ProjectDeliverablesDTO dto)
        => _repo.UpdateProjectDeliverableAsync(deliverableId, dto);

    public Task DeleteProjectDeliverable(int deliverableId)
        => _repo.DeleteProjectDeliverableAsync(deliverableId);
}
