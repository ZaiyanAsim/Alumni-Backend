using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Project.Application.Abstractions;
using Project.Application.DTO;
using Project.Infrastructure.Data_Models;
using Project.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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

        private static Expression<Func<Projects, projectDTO>> ProjectToDTO()
        {
            return p => new projectDTO
            {
                Project_Academic_ID = p.Project_Academic_ID,
                Project_Name = p.Project_Name,
                //Project_Category = p.Project_Category ?? "N/A",
                Project_Type = p.Project_Type_Value,
                Project_Year = p.Project_Year,
                Is_Mentored = p.Is_Mentored,
                Is_Sponsored = p.Is_Sponsored
            };
        }


        public IQueryable<projectDTO> getProjects(ProjectFilters filters)
        {
            
            return _context.Projects
                          .Where(p => filters.Types == null || filters.Types.Contains(p.Project_Type_ID))
                          .Select(ProjectToDTO())
                          .OrderByDescending(p => p.Project_Year);
        }

        public async Task<projectDTO?> getProjectsByAcademicID(string projectAcademicID)
        {
            var project = await _context.Projects
                          .Where(p => p.Project_Academic_ID == projectAcademicID)
                          .Select(ProjectToDTO())
                          .FirstOrDefaultAsync();

           

            return project;
        }

        //public async Task<int> createProject(Projects newProject)
        //{
        //    using var transaction = await _context.Database.BeginTransactionAsync();
        //    try
        //    {
        //        _context.Projects.Add(newProject);
                


        //    }
        //}

        

    }
}

