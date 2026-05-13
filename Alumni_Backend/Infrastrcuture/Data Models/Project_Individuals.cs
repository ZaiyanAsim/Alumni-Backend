using Alumni_Portal.Infrastructure.Data_Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Alumni_Portal.Infrastrcuture.Data_Models
{
    public class Project_Individuals
    {
        [Key]
        
        public int Project_Individuals_Map_ID { get; set; }

        [Required]
        public Projects Project { get; set; }
        public int Project_ID { get; set; }

        [Required]
        public int Individual_ID { get; set; }

        public string Individual_Role { get; set; } = string.Empty;





    }
}
