using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Application.DTO
{
    
        public class CreateProjectDTO
        {
            public required string Project_Academic_ID { get; set; }
            public required string Project_Name { get; set; }
            
        public required string Project_Type { get; set; }

        public required int Project_Year { get; set; }

            public List<string> Project_Industries { get; set; }



        }

        public class ProjectIndividualDTO
        {
            public required string Individual_Institution_ID { get; set; }
            public required string Individual_Name { get; set; }

            public required string Individual_Type_Value { get; set; }
        }
    }

