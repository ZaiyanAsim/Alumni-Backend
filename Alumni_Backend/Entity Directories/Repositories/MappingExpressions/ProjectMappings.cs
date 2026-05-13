
using Alumni_Portal.Infrastructure.Data_Models;
using Entity_Directories.Services.DTO;
using System.Linq.Expressions;
namespace Entity_Directories.Repositories.MappingExpressions
{
    internal class MappingExtension
    {
        public Expression<Func<CreateProjectDTO, Projects>> CreateMapping()
        {
            return p => new Projects
            {
                Project_Academic_ID = p.Project_Academic_ID,
                Project_Name = p.Project_Name,
                Project_Type_ID= p.Project_Type_ID,
                Project_Type_Value = p.Project_Type_Value,
                Project_Year = p.Project_Year,
                Project_Industry = p.Project_Industries.Select(i => new Project_Industry
                {
                    
                }).ToList()

            };
        }
    }
}
