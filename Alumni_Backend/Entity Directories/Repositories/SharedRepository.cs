using Alumni_Portal.Infrastrcuture.Data_Models;
using Alumni_Portal.Infrastructure.Persistance;
using Alumni_Portal.Infrastructure.Persistence;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Entity_Directories.Services.DTO;
using Shared.Custom_Exceptions.ExceptionClasses;

namespace Alumni_Portal.Entity_Directories.Repositories
{
    public class SharedRepository
    {
        private IndividualDbContext _individualContext;
        private ProjectDbContext _projectContext;

        public SharedRepository(IndividualDbContext individual_context, ProjectDbContext project_context)
        {
            _individualContext = individual_context;
            _projectContext = project_context;
        }

        public async Task<int> Individual_Exists_Async(string individualInstitutionID)
        {
            int Individual_ID = await _individualContext.Individuals.
                 Where(i => i.Individual_Institution_ID == individualInstitutionID)
                 .Select(i => i.Individual_ID).FirstOrDefaultAsync();

            return Individual_ID;
            



        }

        public async Task<int> Project_Exists_Async(string projectAcademicID)
        {
            int projectId =
                await _projectContext.Projects.
                Where(p => p.Project_Academic_ID == projectAcademicID)
                .Select(p => p.Project_ID).FirstOrDefaultAsync();
            

            return projectId;
        }

        public async Task Individual_Has_ProjectAsync(string individualInstitutionID,int individualID,int projectID)
        {
            var type = await _individualContext.Individuals.
                Where(i => i.Individual_ID == individualID).
                Select(i => i.Individual_Type_Value).FirstAsync();

            

            int hasProject =
                await _projectContext.Project_Individuals
                .Where(pi => pi.Individual_ID == individualID)
                .Select(pi => pi.Project_ID)

            .FirstOrDefaultAsync();

            if (type == "Student" && hasProject != 0)
            {

                throw new ValidationException($"Student with ID ${individualInstitutionID} is already associated with a project.");
            }
            if ( hasProject == projectID)
            {
                throw new ValidationException($"Individual with id ${individualInstitutionID}  is already associated with this project.");

            }
            


            //bool hasProject =
            //    await _projectContext.Project_Individuals
            //    .Join(
            //        _individualContext.Individuals, // Join with Individuals table
            //        pi => pi.Individual_ID,      
            //        ind => ind.Individual_ID,    
            //        (pi, ind) => new { ProjectIndividual = pi, Individual = ind } 
            //    )
            //    .Where(x => x.Individual.Individual_Type_Value != "Student") 
            //    .AnyAsync(x => x.Individual.Individual_ID == individualID);  

            //if (hasProject)
            //{
            //    throw new ValidationException($"Individual with ID {individualID} is already associated with a project.");
            //}
        }

        public async Task AddProjectMembersAsync(List<Project_Individuals> members)
        {

            _projectContext.Project_Individuals.AddRange(members);


            await _projectContext.SaveChangesAsync();



        }

        public async Task RollBackAsync(DbContext context)
        {
            await context.DisposeAsync();
        }

        public async Task<int> CountAsync<T>(IQueryable<T> query)
        {
            return await query.CountAsync();

        }
    }
}
