using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;


namespace Entity_Directories.Services.DTO
{
    public record userDirectoryDTO
    {

        public int Individual_ID { get; set; }

        public string? Individual_Academic_ID { get; set; }

       
        public required string Individual_Name { get; set; }

        public string? Individual_Email { get; init; }



        
        public int? noMentorships { get; init; }


        
        public int? noSponsorships { get; init; }


        
        public string? Individual_Current_Industry { get; set; }

        
        public string? Individual_Current_Role { get; init; }

        public ProgramInfoDTO? Program { get; init; }

    }


        
    public class ProgramInfoDTO
      
    {
        public string? Program { get; set; }
        public string? Department { get; init; }
        public int? Enrollment_Year { get; init; }
        public int? Graduation_Year { get; init; }
        public string? Designation { get; init; }
    }


        




  






      

        
      
        

    public class BaseUserDTO
    {
        public string Individual_Name { get; set; }
        public string Individual_Email { get; set; }
    }

    // Alumni-specific DTO
    public class AlumniDirectoryDTO : BaseUserDTO
    {
        public string Individual_Current_Industry { get; set; }
        public string Individual_Current_Role { get; set; }
        public int noMentorships { get; set; }
        public int noSponsorships { get; set; }
        public IEnumerable<AlumniProgramInfoDTO> Programs { get; set; }
    }

    public class AlumniProgramInfoDTO
    {
        public string Program { get; set; }
        public int Graduation_Year { get; set; }
        public string Department { get; set; }
    }

    // Student-specific DTO
    public class StudentDirectoryDTO : BaseUserDTO
    {
        public IEnumerable<StudentProgramInfoDTO> Programs { get; set; }
    }

    public class StudentProgramInfoDTO
    {
        public string Program { get; set; }
        public int Graduation_Year { get; set; }
        public string Department { get; set; }
    }

    // Supervisor-specific DTO
    public class SupervisorDirectoryDTO : BaseUserDTO
    {
        public IEnumerable<SupervisorProgramInfoDTO> Programs { get; set; }
    }

    public class SupervisorProgramInfoDTO
    {
        public string Department { get; set; }
        public string Designation { get; set; }
    }
}

