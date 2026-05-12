using System.ComponentModel.DataAnnotations;

namespace Alumni_Portal.Infrastructure.Data_Models
{
    public class Registration_Request
    {
        [Key]
        public int Request_ID { get; set; }

        public required string User_Type { get; set; }        // student | alumni | supervisor

        public required string First_Name { get; set; }
        public required string Last_Name { get; set; }
        public required string Email { get; set; }
        public required string Phone { get; set; }
        public required string Password_Hash { get; set; }

        // Academic (student / alumni)
        public string? Roll_Number { get; set; }
        public int?    Department_ID { get; set; }
        public string? Department_Value { get; set; }
        public int?    Enrollment_Year { get; set; }
        public int?    Graduation_Year { get; set; }

        // Student optional current job
        public string? Current_Job_Company { get; set; }
        public string? Current_Job_Role { get; set; }

        // Alumni work experience (JSON array)
        public string? Work_Experience_JSON { get; set; }

        // Supervisor institutional
        public int?    Designation_ID { get; set; }
        public string? Designation_Value { get; set; }

        public string Status { get; set; } = "Pending";   // Pending | Approved | Rejected
        public DateTime Submitted_At { get; set; } = DateTime.UtcNow;
        public DateTime? Reviewed_At { get; set; }
    }
}
