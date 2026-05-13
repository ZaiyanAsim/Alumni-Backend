using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Infrastructure.Data_Models
{
    internal class Projects
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Project_ID { get; set; }

        [Required]
        public int Client_ID { get; set; }

        [MaxLength(100)]
        public string? Client_Reference_Key { get; set; }

        [Required]
        public int Campus_ID { get; set; }

        [MaxLength(100)]
        public string? Campus_Reference_Key { get; set; }

        [Required]
        [MaxLength(50)]
        public string Project_Academic_ID { get; set; } = string.Empty;

        public int? Project_Type_ID { get; set; }

        [Required]
        [MaxLength(100)]
        public string Project_Type_Value { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string Project_Name { get; set; } = string.Empty;

        [Required]
        public int Project_Year { get; set; }

        [MaxLength(200)]
        public string? Project_Category { get; set; }

        public bool? Is_Mentored { get; set; } = false;

        public bool? Is_Sponsored { get; set; } = false;

        public int? Progress_ID { get; set; }

        [MaxLength(50)]
        public string? Progress_Value { get; set; }

        public int? Status_ID { get; set; }

        [MaxLength(50)]
        public string? Status_Value { get; set; }


        public int? Created_By_ID { get; set; }

        [MaxLength(100)]
        public string? Created_By_Name { get; set; }

        [Column(TypeName = "datetime2(7)")]
        public DateTime? Created_Date { get; set; }

        public int? Updated_By_ID { get; set; }

        [MaxLength(100)]
        public string? Updated_By_Name { get; set; }

        [Column(TypeName = "datetime2(7)")]
        public DateTime? Updated_Date { get; set; }
    }
}
