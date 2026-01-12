namespace Admin.Application.DTO
{


    public record userDirectoryDTO
    {

        public required string Individual_Institution_ID { get; set; }
        public required string Individual_Name { get; set; }
        public string? Individual_Email { get; init; }



        public int? noMentorships { get; init; }

        public int? noSponsorships { get; init; }

        public string? Individual_Current_Industry { get; set; }

        public string? Individual_Current_Role { get; init; }

        public List<ProgramInfoDTO>? Programs { get; init; }

    }

    public record projectDTO
    {
        public required string Project_Academic_ID { get; set; }
        public required string Project_Name { get; init; }

        public int? Project_Year { get; init; }

        public required string Project_Type { get; init; }

        public required string Project_Category { get; init; }

        public bool? Is_Mentored { get; init; }

        public bool? Is_Sponsored { get; init; }

    }







    public class UserFilterParams
    {


    }


    public class NewUserDTO
    {
        public required string Individual_Insititution_ID { get; set; }
        public required string Individual_Name { get; set; }
        public string? Individual_Email { get; init; }

        public required string Individual_Type_Value { get; set; }
        public string? Individual_Contact_Number_Primary { get; init; }
        public bool Individual_Is_Alumni { get; init; }


        public List<ProgramInfoDTO>? Academic_Details { get; init; }

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


