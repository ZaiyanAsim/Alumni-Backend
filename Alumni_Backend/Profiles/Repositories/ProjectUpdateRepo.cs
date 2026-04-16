using Alumni_Portal.Infrastructure.Data_Models;
using Alumni_Portal.Infrastructure.Persistence;
using Alumni_Portal.Profiles.DTO;
using static Alumni_Portal.Profiles.Repositories.MappingExpressions.ProjectProfileMapping;

public class ProjectUpdateRepo
{
    private readonly ProjectDbContext _context;

    public ProjectUpdateRepo(ProjectDbContext context) => _context = context;

    // ── Attachments ───────────────────────────────────────────────────────────

    public async Task AddProjectAttachmentAsync(Project_Attachments attachment)
    {
        try
        {
            await _context.Project_Attachments.AddAsync(attachment);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred while adding the project attachment.", ex);
        }
    }

    public async Task AddProjectMediaAsync(List<Project_Media> mediaRecords)
    {
        try
        {
            await _context.Project_Media.AddRangeAsync(mediaRecords);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred while saving project media.", ex);
        }
    }  
    public async Task DeleteProjectAttachmentAsync(int attachmentId)
    {
        try
        {
            var attachment = await _context.Project_Attachments.FindAsync(attachmentId)
                ?? throw new Exception("Attachment not found.");

            _context.Project_Attachments.Remove(attachment);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred while deleting the project attachment.", ex);
        }
    }

    // ── Results ───────────────────────────────────────────────────────────────

    public async Task AddProjectResultAsync(int projectId, ProjectResultsDTO dto)
    {
        try
        {
            var model = ProjectModelMapping.ToResultModel(projectId, dto);
            await _context.Project_Results.AddAsync(model);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred while adding the project result.", ex);
        }
    }

    //public async Task UpdateProjectResultAsync(int projectId, int resultId, ProjectResultsDTO dto)
    //{
    //    try
    //    {
    //        var existing = await _context.Project_Results.FindAsync(resultId)
    //            ?? throw new Exception("Result not found.");

    //        // Map the DTO onto the tracked entity manually —
    //        // we cannot use ToResultModel here as it creates a new object,
    //        // which would cause EF to lose track of the original row.
    //        existing.Result_Title = dto.Title;
    //        existing.Result_Description = dto.Description;
    //        existing.Result_Type_Value = dto.Type_Value!;
    //        existing.Result_Image_Url = dto.Image_Url;
    //        existing.Result_Seq_Number = dto.Seq_Number;
    //        existing.Result_Metric_Value = dto.MetricValue;
    //        existing.Result_Metric_Label = dto.MetricLabel;
    //        existing.Result_Link = dto.Link;
    //        existing.Result_Tags = dto.Tags;

    //        await _context.SaveChangesAsync();
    //    }
    //    catch (Exception ex)
    //    {
    //        throw new Exception("An error occurred while updating the project result.", ex);
    //    }
    //}

    public async Task DeleteProjectResultAsync(int resultId)
    {
        try
        {
            var result = await _context.Project_Results.FindAsync(resultId)
                ?? throw new Exception("Result not found.");

            _context.Project_Results.Remove(result);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred while deleting the project result.", ex);
        }
    }

    // ── Deliverables ──────────────────────────────────────────────────────────

    public async Task AddProjectDeliverableAsync(int projectId, ProjectDeliverablesDTO dto)
    {
        try
        {
            var model = ProjectModelMapping.ToDeliverableModel(projectId, dto);
            await _context.Project_Deliverables.AddAsync(model);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred while adding the project deliverable.", ex);
        }
    }

    //public async Task UpdateProjectDeliverableAsync(int projectId, int deliverableId, ProjectDeliverablesDTO dto)
    //{
    //    try
    //    {
    //        var existing = await _context.Project_Deliverables.FindAsync(deliverableId)
    //            ?? throw new Exception("Deliverable not found.");

    //        // Same reason as results — mutate the tracked entity directly
    //        existing.Deliverable_Title = dto.Title;
    //        existing.Deliverable_Description = dto.Description;
    //        existing.Deliverable_Status_Value = dto.Status_Value!;
    //        existing.Deliverable_Category_Value = dto.Category_Value!;
    //        existing.Date = dto.Date;

    //        await _context.SaveChangesAsync();
    //    }
    //    catch (Exception ex)
    //    {
    //        throw new Exception("An error occurred while updating the project deliverable.", ex);
    //    }
    //}

    public async Task DeleteProjectDeliverableAsync(int deliverableId)
    {
        try
        {
            var deliverable = await _context.Project_Deliverables.FindAsync(deliverableId)
                ?? throw new Exception("Deliverable not found.");

            _context.Project_Deliverables.Remove(deliverable);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred while deleting the project deliverable.", ex);
        }
    }
}