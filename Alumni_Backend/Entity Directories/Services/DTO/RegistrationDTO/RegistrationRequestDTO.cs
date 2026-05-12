namespace Entity_Directories.Services.DTO
{
    public class SubmitRegistrationRequestDTO
    {
        public required string UserType { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Email { get; set; }
        public required string Phone { get; set; }
        public required string Password { get; set; }

        // Academic
        public string? RollNumber { get; set; }
        public int?    DepartmentId { get; set; }
        public string? DepartmentValue { get; set; }
        public int?    EnrollmentYear { get; set; }
        public int?    GraduationYear { get; set; }

        // Student optional job
        public string? CurrentJobCompany { get; set; }
        public string? CurrentJobRole { get; set; }

        // Alumni work experience
        public List<WorkExpSubmitDTO>? WorkExperience { get; set; }

        // Supervisor
        public int?    DesignationId { get; set; }
        public string? DesignationValue { get; set; }
    }

    public class WorkExpSubmitDTO
    {
        public string? Company { get; set; }
        public string? Role { get; set; }
        public string? StartDate { get; set; }
        public string? EndDate { get; set; }
        public bool    IsCurrent { get; set; }
    }

    public class RegistrationRequestListDTO
    {
        public int    RequestId { get; set; }
        public string UserType { get; set; } = "";
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public string Email { get; set; } = "";
        public string Phone { get; set; } = "";
        public string Status { get; set; } = "";
        public DateTime SubmittedAt { get; set; }

        // Academic
        public string? RollNumber { get; set; }
        public string? DepartmentValue { get; set; }
        public int?    EnrollmentYear { get; set; }
        public int?    GraduationYear { get; set; }
        public string? CurrentJobCompany { get; set; }
        public string? CurrentJobRole { get; set; }
        public string? WorkExperienceJson { get; set; }

        // Supervisor
        public string? DesignationValue { get; set; }
    }
}
