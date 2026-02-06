using Alumni_Portal.Entity_Directories.Repositories;
using Alumni_Portal.Infrastrcuture.Data_Models;
using Alumni_Portal.Infrastructure.Data_Models;
using Entity_Directories.Services.Abstractions;
using Entity_Directories.Services.DTO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Shared.Custom_Exceptions.ExceptionClasses;
using System.Runtime.CompilerServices;

namespace Entity_Directories.Services
{

    public class ProjectService
    {
        private IProjectRepository _projectRepo;

        private SharedRepository _sharedRepo;
        public ProjectService(IProjectRepository projectRepo, SharedRepository sharedRepo)
        {
            _projectRepo = projectRepo;
            _sharedRepo = sharedRepo;
        }

        public async Task<projectDTO?> GetProject(string projectAcademicID)
        {
            
            return await _projectRepo.getProjectsByAcademicID(projectAcademicID.ToUpper());
        }

        public async Task<PaginatedResult<projectDTO>> GetProjectsPaginated(ProjectFilters filters, int _page, int _limit)
        {
            
            if (_page > 0 || _limit > 0)
            {


                var query =  _projectRepo.getProjects(filters);

                int count = await _sharedRepo.CountAsync(query);
                
               var projects =await query.
                    Skip((_page-1)*_limit)
                    .Take(_limit)
                    .ToListAsync();

                return new PaginatedResult<projectDTO>
                {
                    data = projects,
                    totalRecords = count,
                    _page = _page,
                    _size = _limit

                };
            }

           

            throw new ValidationException("Page and Limit must be greater than 0");

        }

        public async Task<int> CreateProject(CreateProjectDTO newProject)
        {
            int id=await _sharedRepo.Project_Exists_Async(newProject.Project_Academic_ID);

            if (id != 0)
            {
                throw new ValidationException("Project with this id already exists");
            }
            
            var project = new Projects
            {
                Client_ID = 1,
                Campus_ID=1,
                Project_Academic_ID = newProject.Project_Academic_ID,
                Project_Name = newProject.Project_Name,
                Project_Type_ID=newProject.Project_Type_ID,
                Project_Type_Value = newProject.Project_Type_Value,
                Project_Year = newProject.Project_Year,
                Project_Industries = newProject.Project_Category, // directory specific parameter
                Project_Industry = newProject.Project_Industries.Select(i => new Project_Industry
                {
                    Project_Industry_Value = i.Industry_Name,
                    Project_Industry_Parameter_ID = i.Parameter_ID,

                }).ToList()

            };


            int project_ID = await _projectRepo.CreateAsync(project);
            


            return project_ID;

        }
        
        public async Task AddProjectMembers( string project_ID, List<ProjectIndividualDTO> members) {

            int projectid = await _sharedRepo.Project_Exists_Async(project_ID);

            if (projectid == 0)
            {
                throw new ValidationException("Project with this ID does not exist");
            }
            
            
            var project_Individuals = new List<Project_Individuals>();
            foreach (var member in members)
            {
                int id = await _sharedRepo.Individual_Exists_Async(member.Individual_Institution_ID);
                if (id == 0)
                {
                    throw new ValidationException("Individual with ID " + member.Individual_Institution_ID + " does not exist.");
                }
                await _sharedRepo.Individual_Has_ProjectAsync(member.Individual_Institution_ID,id,projectid);

                 project_Individuals.Add(  new Project_Individuals
                 {

                     Project_ID = projectid,
                     Individual_ID = id,
                     
                 });

            }
            await _sharedRepo.AddProjectMembersAsync(project_Individuals);
            
            
        }

        public async Task<List<int>> DeleteProjectsBulk(List<int> projectIDs)
        {
            

               return await _projectRepo.DeleteBulkAsync(projectIDs);
              
        }

            

            





    }
}
