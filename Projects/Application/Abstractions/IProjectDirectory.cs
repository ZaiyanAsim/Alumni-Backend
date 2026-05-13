using Project.Application.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Application.Abstractions
{
    public interface IProjectDirectory
    {
        public IQueryable<projectDTO> getProjects(ProjectFilters filters);
        public Task<projectDTO?> getProjectsByAcademicID(string projectAcademicID);
    }
}
