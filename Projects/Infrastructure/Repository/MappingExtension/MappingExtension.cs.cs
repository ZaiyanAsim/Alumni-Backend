using Project.Application.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Project.Infrastructure.Data_Models;

namespace Project.Infrastructure.Repository.MappingExtension
{
    internal class MappingExtension
    {
        public Expression<Func<CreateProjectDTO, Projects>> CreateMapping()
        {
            return p => new Projects
            {
                Project_Academic_ID = p.Project_Academic_ID,
                Project_Name = p.Project_Name,

                Project_Type_Value = p.Project_Type,
                Project_Year = p.Project_Year,
                
            };
        }
    }
}
