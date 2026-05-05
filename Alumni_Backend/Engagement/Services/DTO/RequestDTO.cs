using Alumni_Portal.Infrastructure.Data_Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Alumni_Portal.Engagement.Services.DTO
{
    public class RequestDTO
    {
       

        public int? Request_Type_ID { get; set; }
        public string? Request_Type_Value { get; set; }

        public int Project_ID { get; set; }
        public string Project_Academic_ID { get; set; } = null!;
        public string Project_Name { get; set; } = null!;

        public bool Is_Individual_Registered { get; set; }

        public int? Individual_ID { get; set; }
        public string? Individual_Institution_ID { get; set; }

        public string? Individual_Name { get; set; }
        public string? Individual_Email { get; set; }

        public string? Individual_Contact_Number { get; set; }

        public string? Individual_LinkedIn_Url { get; set; }






        public bool Is_Organization { get; set; }
        public string? Organization_Name { get; set; }
        public string? Organization_Role { get; set; }


        public string? Motivation_Statement { get; set; }


        public int? Status_ID { get; set; }
        public string? Status_Value { get; set; }


        public DateTime Created_At { get; set; }
      
    }
}
