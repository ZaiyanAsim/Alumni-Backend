using Alumni_Portal.Infrastructure.Data_Models;
using Alumni_Portal.Infrastructure.Persistance;
using Alumni_Portal.Infrastructure.Persistence;
using Alumni_Portal.Profiles.DTO;

using Microsoft.EntityFrameworkCore;
using Alumni_Portal.Profiles.Repositories.MappingExpressions;

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
            .Select(ProjectProfileMapping.ToMetaDataDTO)
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
                        pm.Individual_Role,
                        i.Individual_Email,
                        i.Logo_Url
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
            .Select(ProjectProfileMapping.ToProjectDocumentDto)
            .ToListAsync();

         
            rows.ForEach(r => r.FileType = Path.GetExtension(r.FileName).TrimStart('.').ToUpperInvariant());

            return rows;
        }

        public async Task<List<ProjectResultsDTO>> GetResultsDTOs(int projectId)
        {
            return await _projectDbContext.Project_Results
        .AsNoTracking()
        .Where(r => r.Project_ID == projectId)
        .OrderByDescending(r => r.Result_Seq_Number)
        .Select(ProjectProfileMapping.ToProjectResultsDTO)
        .ToListAsync(); ;
        }

        public async Task<List<TechStackDTO>> GetTechStackAsync(int projectId)
        {
            return await _projectDbContext.Project_Tech_Stack
            .AsNoTracking()
            .Where(t => t.Project_ID == projectId)
            .Select(ProjectProfileMapping.ToTechStackDTO)
            .ToListAsync();
        }

        public async Task<List<ProjectDeliverablesDTO>> GetDeliverablesDTOs(int projectId)
        {
            return await _projectDbContext.Project_Deliverables
            .AsNoTracking()
            .Where(d => d.Project_ID == projectId)
            .OrderByDescending(d => d.Date)
            .Select(ProjectProfileMapping.ToProjectDeliverablesDTO)
            .ToListAsync();
        }









    }
}

