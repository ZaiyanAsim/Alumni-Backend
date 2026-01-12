using Admin.Application.Abstractions;
using Admin.Application.DTO;
using Admin.Infrastructure.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Identity.Client;

namespace Admin.Infrastructure.Repositories
{
    public class projectRepository: IProjectService
    {

        private AppDbContext _context;
         public projectRepository( AppDbContext context)
        {
            _context = context;
        }
        
           public IQueryable<projectDTO> getProjects(string type)
        {

            try
            {
                var projects = _context.Projects
                              .Where(p => p.Project_Type_Value == type)
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

