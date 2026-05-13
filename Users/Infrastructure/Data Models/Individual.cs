using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Users.Infrastructure.Data_Models
{
    public record Individuals
    {
        [Key]
        public int Individual_ID { get; init; }
        public required string Individual_Institution_ID { get; init; }
        public int? Client_ID { get; init; }
        public string? Client_Reference_Key { get; init; }
        public int? Campus_ID { get; init; }
        public string? Campus_Reference_Key { get; init; }
        public int? Individual_Type_Id { get; init; }
        public required string Individual_Type_Value { get; init; }


        public required string Individual_Name { get; init; }
        public string? Individual_Email { get; init; }
        public string? Individual_Contact_Number_Primary { get; init; }
        public string? Individual_Contact_Number_Secondary { get; init; }


        public required bool Individual_Is_Alumni { get; init; }


        public int? Individual_Mentorship_Count { get; init; }
        public int? Individual_Sponsorship_Count { get; init; }

        public string? Individual_Current_Industry { get; init; }

        public string? Individual_Current_Role { get; init; }

        public int? Progress_ID { get; set; }
        public string? Progress_Value { get; set; }

        public int? Status_ID { get; set; }
        public string? Status_Value { get; set; }

        public virtual ICollection<Individual_Academics> Academic_Details { get; set; }

    }

    public class Individual_Academics
    {
        [Key]
        public int Individual_Academic_ID { get; set; }
        public int Individual_ID { get; set; }

        public string? Individual_Academic_Student_ID { get; set; }
        public string? Individual_Academic_Batch { get; set; }

        public int? Individual_Academic_Enrollment_Year { get; set; }
        public int? Individual_Academic_Graduation_Year { get; set; }

        public int? Individual_Academic_Program_ID { get; set; }
        public string? Individual_Academic_Program_Value { get; set; }

        public int? Individual_Academic_Department_ID { get; set; }
        public string? Individual_Academic_Department_Value { get; set; }



        public string? Individual_Academic_Designation { get; set; }

        public int? Progress_ID { get; set; }
        public string? Progress_Value { get; set; }

        public int? Status_ID { get; set; }
        public string? Status_Value { get; set; }


    }
}
