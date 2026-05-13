using Entity_Directories.Services.DTO;
using Alumni_Portal.Infrastructure.Data_Models;

namespace Entity_Directories.Services.Abstractions
{

    
        public interface IProjectRepository
    {
            public IQueryable<projectDTO>getProjects(ProjectFilters filters);
            public Task<projectDTO?> getProjectsByAcademicID(string projectAcademicID);

            public  Task<int> CreateAsync(Projects project);

        public Task<List<int>> DeleteBulkAsync(List<int> ids);
        }
    }

