    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    

    

    namespace Alumni_Portal.Infrastructure.Data_Models
    {
        [Table("Project_Industry")]
        public class Project_Industry
        {
            [Key]
            [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
            public int Project_Industry_ID { get; set; }
            public Projects Project { get; set; } = null!;
            [Required]
            [ForeignKey("Project_ID")]
            public int Project_ID { get; set; }



            public int? Project_Industry_Parameter_ID { get; set; }

            [Required]
            [MaxLength(100)]
            public string Project_Industry_Value { get; set; } = string.Empty;


        }

    }

            
