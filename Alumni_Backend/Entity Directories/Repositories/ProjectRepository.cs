
using Alumni_Portal.Infrastrcuture.Data_Models;
using Alumni_Portal.Infrastructure.Data_Models;
using Alumni_Portal.Infrastructure.Persistance;
using Alumni_Portal.Infrastructure.Persistence;
using Entity_Directories.Repositories.MappingExpressions;
using Entity_Directories.Services.Abstractions;
using Entity_Directories.Services.DTO;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Shared.Custom_Exceptions.ExceptionClasses;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Entity_Directories.Repositories
{

    internal class ProjectRepository : IProjectRepository
    {
        private ProjectDbContext _context;
        private IndividualDbContext _individualContext;
        public ProjectRepository(ProjectDbContext context, IndividualDbContext individualContext)
        {
            _context = context;
            _individualContext = individualContext;
        }

        private static Expression<Func<Projects, projectDTO>> ProjectToDTO()
        {
            return p => new projectDTO
            {
                Project_ID=p.Project_ID,
                Project_Academic_ID = p.Project_Academic_ID,
                Project_Name = p.Project_Name,
                Project_Category = p.Project_Industries ?? "N/A",
                Project_Type = p.Project_Type_Value,
                Project_Year = p.Project_Year,
                Is_Mentored = p.Is_Mentored,
                Is_Sponsored = p.Is_Sponsored
            };
        }


        public IQueryable<projectDTO> getProjects(ProjectFilters filters)
        {

            var projects = _context.Projects
                          .Where(p => filters.Types == null || filters.Types.Count == 0 || filters.Types.Contains(p.Project_Type_ID))
                          .Select(ProjectToDTO())
                          .OrderByDescending(p => p.Project_Year);


            return projects;

          
        }

        public async Task<projectDTO?> getProjectsByAcademicID(string projectAcademicID)
        {
            var project = await _context.Projects
                          .Where(p => p.Project_Academic_ID == projectAcademicID)
                          .Select(ProjectToDTO())
                          .FirstOrDefaultAsync();

            return project;



        }

        public async Task<int> CreateAsync(Projects project)
        {

            _context.Projects.Add(project);
            await _context.SaveChangesAsync();

            return project.Project_ID;



        }



        public async Task AddProjectMembersAsync(List<Project_Individuals> members)
        {

            _context.Project_Individuals.AddRange(members);
            _context.SaveChangesAsync();

        }

        public async Task RollBackAsync()
        {
            await _context.DisposeAsync();
        }






        //public async Task<List<int>> DeleteBulkAsync(List<int> projectIds)
        //{

        //    var failedDeletes = new List<int>();

        //foreach (var projectId in projectIds)
        //    {
        //        try
        //        {
        //            await _context.Projects
        //                .Where(p => p.Project_ID == projectId)
        //                .ExecuteDeleteAsync();


        //        }
        //        catch (Exception ex)

        //        {

        //            failedDeletes.Add(projectId);
        //        }
        //    }

        //   return failedDeletes;
        //}

        public async Task<List<int>> DeleteBulkAsync(List<int> projectIds)
        {
            

            try
            {
                // Build parameterized query
                var parameters = projectIds.Select((id, index) =>
                    new SqlParameter($"@p{index}", id)
                ).ToArray();

                var parameterNames = string.Join(",",
                    parameters.Select(p => p.ParameterName)
                );

                // Delete and get successfully deleted IDs
                var deletedIds = await _context.Database
                    .SqlQueryRaw<int>(
                        $@"DELETE FROM Projects
                   OUTPUT DELETED.Project_ID
                   WHERE Project_ID IN ({parameterNames})",
                        parameters
                    )
                    .ToListAsync();

               
                var failedIds = projectIds.Except(deletedIds).ToList();

                return failedIds;
            }
            catch (Exception)
            {
                // If the entire delete fails, all IDs failed
                return projectIds;
            }
        }

    }


    }


    
