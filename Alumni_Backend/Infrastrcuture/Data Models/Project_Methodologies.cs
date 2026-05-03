using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Alumni_Portal.Infrastructure.Data_Models
{
    [Table("Project_Methodologies")]
    public class Project_Methodologies
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Project_Methodology_ID { get; set; }

        [Required]
        public int Project_ID { get; set; }

        [Required]
        [MaxLength(100)]
        public string Methodology_Value { get; set; } = string.Empty;
    }
}
