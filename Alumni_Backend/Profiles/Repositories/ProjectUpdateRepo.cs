using Alumni_Portal.Infrastrcuture.Data_Models;
using Alumni_Portal.Infrastructure.Data_Models;
using Alumni_Portal.Infrastructure.Persistence;
using Alumni_Portal.Profiles.DTO;
using Microsoft.EntityFrameworkCore;
using static Alumni_Portal.Profiles.Repositories.MappingExpressions.ProjectProfileMapping;

public class ProjectUpdateRepo
{
    private readonly ProjectDbContext _context;

    public ProjectUpdateRepo(ProjectDbContext context) => _context = context;

    // ── Attachments ───────────────────────────────────────────────────────────

    public async Task AddProjectAttachmentAsync(Project_Attachments attachment)
    {
        await _context.Project_Attachments.AddAsync(attachment);
        await _context.SaveChangesAsync();
    }

    public async Task AddProjectMediaAsync(List<Project_Media> mediaRecords)
    {
        await _context.Project_Media.AddRangeAsync(mediaRecords);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteProjectAttachmentAsync(int attachmentId)
    {
        var attachment = await _context.Project_Attachments.FindAsync(attachmentId)
            ?? throw new Exception("Attachment not found.");
        _context.Project_Attachments.Remove(attachment);
        await _context.SaveChangesAsync();
    }

    // ── Members ───────────────────────────────────────────────────────────────

    public async Task<int> AddMemberAsync(int projectId, int individualId, string role)
    {
        var entry = new Project_Individuals
        {
            Project_ID = projectId,
            Individual_ID = individualId,
            Individual_Role = role,
        };
        await _context.Project_Individuals.AddAsync(entry);
        await _context.SaveChangesAsync();
        return entry.Project_Individuals_Map_ID;
    }

    public async Task RemoveMemberAsync(int mapId)
    {
        var entry = await _context.Project_Individuals.FindAsync(mapId)
            ?? throw new Exception("Member not found.");
        _context.Project_Individuals.Remove(entry);
        await _context.SaveChangesAsync();
    }

    // ── Tech Stack ─────────────────────────────────────────────────────────────

    public async Task<int> AddTechStackAsync(int projectId, string technologyValue, string? layerValue)
    {
        var entry = new Project_Tech_Stack
        {
            Project_ID = projectId,
            Technology_Value = technologyValue,
            Layer_Value = layerValue,
        };
        await _context.Project_Tech_Stack.AddAsync(entry);
        await _context.SaveChangesAsync();
        return entry.Project_Stack_ID;
    }

    public async Task RemoveTechStackAsync(int stackId)
    {
        var entry = await _context.Project_Tech_Stack.FindAsync(stackId)
            ?? throw new Exception("Tech stack item not found.");
        _context.Project_Tech_Stack.Remove(entry);
        await _context.SaveChangesAsync();
    }

    // ── Methodologies ─────────────────────────────────────────────────────────

    public async Task<int> AddMethodologyAsync(int projectId, string methodologyValue)
    {
        var entry = new Project_Methodologies
        {
            Project_ID = projectId,
            Methodology_Value = methodologyValue,
        };
        await _context.Project_Methodologies.AddAsync(entry);
        await _context.SaveChangesAsync();
        return entry.Project_Methodology_ID;
    }

    public async Task RemoveMethodologyAsync(int methodologyId)
    {
        var entry = await _context.Project_Methodologies.FindAsync(methodologyId)
            ?? throw new Exception("Methodology not found.");
        _context.Project_Methodologies.Remove(entry);
        await _context.SaveChangesAsync();
    }

    // ── Description ───────────────────────────────────────────────────────────

    public async Task UpdateDescriptionAsync(int projectId, string description)
    {
        await _context.Projects
            .Where(p => p.Project_ID == projectId)
            .ExecuteUpdateAsync(s => s.SetProperty(p => p.Project_Description, description));
    }

    // ── Results ───────────────────────────────────────────────────────────────

    public async Task AddProjectResultAsync(int projectId, ProjectResultsDTO dto)
    {
        var model = ProjectModelMapping.ToResultModel(projectId, dto);
        await _context.Project_Results.AddAsync(model);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateProjectResultAsync(int resultId, ProjectResultsDTO dto)
    {
        var existing = await _context.Project_Results.FindAsync(resultId)
            ?? throw new Exception("Result not found.");
        existing.Result_Title = dto.Title;
        existing.Result_Description = dto.Description;
        existing.Result_Type_Value = dto.Type_Value!;
        existing.Result_Image_Url = dto.Image_Url;
        existing.Result_Metric_Value = dto.MetricValue;
        existing.Result_Metric_Label = dto.MetricLabel;
        existing.Result_Link = dto.Link;
        existing.Result_Tags = dto.Tags;
        existing.Date = dto.Date;
        await _context.SaveChangesAsync();
    }

    public async Task DeleteProjectResultAsync(int resultId)
    {
        var result = await _context.Project_Results.FindAsync(resultId)
            ?? throw new Exception("Result not found.");
        _context.Project_Results.Remove(result);
        await _context.SaveChangesAsync();
    }

    // ── Deliverables ──────────────────────────────────────────────────────────

    public async Task AddProjectDeliverableAsync(int projectId, ProjectDeliverablesDTO dto)
    {
        var model = ProjectModelMapping.ToDeliverableModel(projectId, dto);
        await _context.Project_Deliverables.AddAsync(model);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateProjectDeliverableAsync(int deliverableId, ProjectDeliverablesDTO dto)
    {
        var existing = await _context.Project_Deliverables.FindAsync(deliverableId)
            ?? throw new Exception("Deliverable not found.");
        existing.Deliverable_Title = dto.Title;
        existing.Deliverable_Description = dto.Description;
        existing.Deliverable_Status_Value = dto.Status_Value!;
        existing.Deliverable_Category_Value = dto.Category_Value!;
        existing.Date = dto.Date;
        await _context.SaveChangesAsync();
    }

    public async Task DeleteProjectDeliverableAsync(int deliverableId)
    {
        var deliverable = await _context.Project_Deliverables.FindAsync(deliverableId)
            ?? throw new Exception("Deliverable not found.");
        _context.Project_Deliverables.Remove(deliverable);
        await _context.SaveChangesAsync();
    }
}
