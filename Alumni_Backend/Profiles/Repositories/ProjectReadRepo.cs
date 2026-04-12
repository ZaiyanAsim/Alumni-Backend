using Alumni_Portal.Infrastructure.Data_Models;
using Alumni_Portal.Infrastructure.Persistance;
using Alumni_Portal.Infrastructure.Persistence;
using Alumni_Portal.Profiles.DTO;
using Microsoft.EntityFrameworkCore;


namespace Alumni_Portal.Profiles.Repositories
{
    public class ProjectReadRepo
    {
        ProjectDbContext _projectDbContext;
        IndividualDbContext _individualDbContext;
        public ProjectReadRepo(ProjectDbContext projectContext, IndividualDbContext individualContext)
        {
            _projectDbContext = projectContext;
            _individualDbContext = individualContext;
        }


        public async Task<object?> GetProjectHeaderAsync(int projectId, CancellationToken ct = default) // Rest details pof header will come from directory. This is better 
        {
            return await _projectDbContext.Projects
                .AsNoTracking()
                .Where(p => p.Project_ID == projectId)
                .Select(p => new MetaDataDTO
                {
                    Project_ID = p.Project_ID,
                    Logo_Url = p.Logo_Url,
                    Project_Academic_ID = p.Project_Academic_ID,
                    Project_Name = p.Project_Name,
                    Project_Type = p.Project_Type_Value,
                    Video_Url = p.Video_Url,
                    Project_Year = p.Project_Year,
                    Project_Category = p.Project_Industries,
                    Project_Description = p.Project_Description,
                    Is_Mentored = p.Is_Mentored,
                    Is_Sponsored = p.Is_Sponsored,
                })
                .FirstOrDefaultAsync(ct);
        }
        public async Task<List<MemberDTO>> GetMembers(int projectId, CancellationToken ct = default)
        {

            var projectMembers = await _projectDbContext.Project_Individuals
                .AsNoTracking()
                .Where(pi => pi.Project_ID == projectId)
                .Select(pi => new { pi.Individual_ID, pi.Individual_Role })
                .ToListAsync(ct);

            if (!projectMembers.Any())
                return new List<MemberDTO>();


            var individualIds = projectMembers.Select(pm => pm.Individual_ID).ToList();

            var individuals = await _individualDbContext.Individuals
                .AsNoTracking()
                .Where(i => individualIds.Contains(i.Individual_ID))
                .Select(i => new { i.Individual_ID, i.Individual_Name, i.Individual_Email, i.Logo_Url })
                .ToListAsync(ct);


            return projectMembers
                .Join(
                    individuals,
                    pm => pm.Individual_ID,
                    i => i.Individual_ID,
                    (pm, i) => new MemberDTO(
                        i.Individual_ID,
                        i.Individual_Name,
                        i.Individual_Email,
                        i.Logo_Url,
                        pm.Individual_Role
                    )
                )
                .ToList();
        }


        public async Task<List<ProjectDocumentDto>> GetDocuments(int projectId)
        {
            var rows = await _projectDbContext.Project_Attachments
                .AsNoTracking()
                .Where(a => a.Project_ID == projectId)
                .OrderByDescending(a => a.Attachment_Date)
                .Select(a => new
                {
                    a.Project_Attachment_ID,
                    a.Attachment_Title,
                    a.Attachment_Description,
                    a.Attachment_File_Location,
                    a.Attachment_File_Name,
                    a.Attachment_Size,
                })
                .ToListAsync();

            return rows.Select(a => new ProjectDocumentDto
            {
                AttachmentId = a.Project_Attachment_ID,
                Title = a.Attachment_Title,
                Description = a.Attachment_Description ?? "",
                FileName = a.Attachment_File_Name ?? "",
                FileUrl = a.Attachment_File_Location ?? "",
                FileType = Path.GetExtension(a.Attachment_File_Name ?? "").TrimStart('.').ToUpperInvariant(),
                FileSize = a.Attachment_Size ?? 0,
            }).ToList();
        }

        public async Task<List<ProjectResultsDTO>> GetResultsDTOs(int projectId)
        {
            return await _projectDbContext.Project_Results
                .AsNoTracking()
                .Where(r => r.Project_ID == projectId)
                .OrderByDescending(r => r.Result_Seq_Number)
                .Select(r => new ProjectResultsDTO
                {
                    Seq_Number = r.Result_Seq_Number ?? 0,
                    Title = r.Result_Title,
                    Description = r.Result_Description!,
                    Type_Value = r.Result_Type_Value!,
                    MetricLabel = r.Result_Metric_Label!,
                    MetricValue = r.Result_Metric_Value!,
                    Link = r.Result_Link!,
                    Tags = r.Result_Tags!,
                    Image_Url = r.Result_Image_Url!,
                })
                .ToListAsync();
        }

        public async Task<List<TechStackDTO>> GetTechStackAsync(int projectId)
        {
            return await _projectDbContext.Project_Tech_Stack
                .AsNoTracking()
                .Where(t => t.Project_ID == projectId)
                .Select(t => new TechStackDTO
                {
                    Layer = t.Layer_Value,
                    Technology = t.Technology_Value,
                })
                .ToListAsync();
        }

        public async Task<List<ProjectDeliverablesDTO>> GetDeliverablesDTOs(int projectId)
        {
            return await _projectDbContext.Project_Deliverables
                .AsNoTracking()
                .Where(d => d.Project_ID == projectId)
                .OrderByDescending(d => d.Date)
                .Select(d => new ProjectDeliverablesDTO
                {
                    Title = d.Deliverable_Title,
                    Description = d.Deliverable_Description ?? "",
                    Status_Value = d.Deliverable_Status_Value!,
                    Category_Value = d.Deliverable_Category_Value!,
                    Date = d.Date
                })
                .ToListAsync();
        }









    }
}

