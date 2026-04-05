using Alumni_Portal.Infrastructure.Data_Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Alumni_Portal.Infrastrcuture.Data_Models
{
    public class Project_Tech_Stack
    {

        [Key]
        public int Project_Stack_ID { get; set; }

        public Projects Project { get; set; } = null!;
        [Required]
        [ForeignKey("Project_ID")]
        public int Project_ID { get; set; }
                                           
        public int? Layer_ID { get; set; }                 
        public string? Layer_Value { get; set; }           
        public int? Technology_ID { get; set; }            
        public string Technology_Value { get; set; } = string.Empty;  

    }
       
     


}
