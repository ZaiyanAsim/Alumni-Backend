using Microsoft.Data.SqlClient;
using Project.Infrastructure.Persistence;
using Project.Application.Abstractions;
using Project.Application.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Infrastructure.Repository
{
    internal class ProjectDirectory:IProjectDirectory   
    {
        private AppDbContext _context;
        public ProjectDirectory(AppDbContext context)
        {
            _context = context;
        }

        public IQueryable<projectDTO> getProjects(ProjectFilters filters)
        {

            try
            {
                var projects = _context.Projects
                              .Where( p=> 
                              filters.Types.Count==0|| filters.Types==null|| filters.Types.Contains(p.Project_Type_ID))
                              .Select(p => new projectDTO
                              {
                                  Project_Academic_ID = p.Project_Academic_ID,
                                  Project_Name = p.Project_Name,
                                  Project_Category = p.Project_Category ?? "N/A",
                                  Project_Type = p.Project_Type_Value,
                                  Project_Year = p.Project_Year,
                                  Is_Mentored = p.Is_Mentored,
                                  Is_Sponsored = p.Is_Sponsored
                                  
                              });

                return projects;

            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception();
            }
        }

    }
}

