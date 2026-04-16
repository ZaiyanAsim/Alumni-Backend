using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Application.DTO
{
    public class ProjectFilters
    {
        public List<int?> Types { get; set; } = [];
        public List<int?> Categories { get; set; } = [];
    }
}
