using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity_Directories.Services.DTO
{
    
        public class CreateProjectDTO
        {
          public required string Project_Academic_ID { get; set; }
          public required string Project_Name { get; set; }
            
          public int Project_Type_ID { get; set; } 
          public required string Project_Type_Value { get; set; }

          public required int Project_Year { get; set; }

          public string Project_Category { get; set; } = string.Empty;
        public List<ProjectIndustryDTO> Project_Industries { get; set; } = null!;

         

    }

        public class ProjectIndividualDTO
        {
            public required string Individual_Institution_ID { get; set; }
            public  string Individual_Role { get; set; }
            

         
        }

    public class ProjectIndustryDTO
    {

        


        public int Parameter_ID { get; set; }

        public string Industry_Name { get; set; }
    }
    }

