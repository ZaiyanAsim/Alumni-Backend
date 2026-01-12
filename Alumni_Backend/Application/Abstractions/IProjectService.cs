using Admin.Application.DTO;

namespace Admin.Application.Abstractions
{
    public interface IProjectService
    {
        public IQueryable<projectDTO> getProjects(string type);
    }
}
