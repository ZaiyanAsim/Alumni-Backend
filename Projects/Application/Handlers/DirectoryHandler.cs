using Project.Application.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Project.Infrastructure.Repository;
using Project.Application.Abstractions;
using Microsoft.EntityFrameworkCore;
namespace Project.Application.Handlers
{
    public class DirectoryHandler
    {
        private IProjectDirectory _projectService;

        public DirectoryHandler(IProjectDirectory projectService)
        {
            _projectService = projectService;
        }


        public async Task<List<projectDTO>> GetProjectsPaginated(ProjectFilters filters, int _page, int _limit)
        {
            try
            {
                var projects = await _projectService.getProjects(filters)
                 .Skip((_page - 1) * _limit)
                 .Take(_limit)
                 .ToListAsync();


                return projects;
            }
            catch (Exception)
            {
                throw new Exception("Error occured while fetching Projects table");
            }
        }

    }
}
