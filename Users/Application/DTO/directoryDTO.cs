using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Users.Application.DTO
{
    public record userDirectoryDTO
    {

       
        public required string Individual_Name { get; set; }
        public string? Individual_Email { get; init; }



        public int? noMentorships { get; init; }

        public int? noSponsorships { get; init; }

        public string? Individual_Current_Industry { get; set; }

        public string? Individual_Current_Role { get; init; }

        public IEnumerable<ProgramInfoDTO>? Programs { get; init; }

    }

    public class ProgramInfoDTO
    {

        public string Program { get; set; }
        public string? Student_ID { get; init; }
        public string? Batch { get; init; }
        public int? Enrollment_Year { get; init; }
        public int? Graduation_Year { get; init; }
        public string? Department { get; init; }
        public string? Designation { get; init; }
    }
}
