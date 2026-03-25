using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Alumni_Portal.TenantConfiguration.Data_Models
{
    
    [Table("Parameter_Values", Schema = "dbo")]
    public class Parameter_Values
    {
        [Key]
        public int Parameter_Value_ID { get; set; }

        public int Client_ID { get; set; }

        [Required]
        [MaxLength(100)]
        public string Client_Reference_Key { get; set; } = string.Empty;

        public int Parameter_ID { get; init; }

        [Required]
        [MaxLength(250)]
        public string Parameter_Reference_Key { get; set; } = string.Empty;

        [Required]
        [MaxLength(250)]
        public string Parameter_Value_Name { get; set; } = string.Empty;

        [MaxLength(100)]
        public string? Parameter_Value_Reference_Key { get; set; }

        [MaxLength(250)]
        public string? Parameter_Value_Description { get; set; }

        public int Parameter_Value_Parent_ID { get; set; }

        public bool Parameter_Value_Is_Header { get; set; }

        public int Parameter_Value_Sequence_Number { get; set; }

        /// <summary>
        /// 1 = Text, 2 = Numeric, 3 = DateTime, 4 = Boolean, 5 = Image
        /// </summary>
        public int Parameter_Value_Type_ID { get; set; }

        [Required]
        [MaxLength(50)]
        public string Parameter_Value_Type_Value { get; set; } = string.Empty;

        [MaxLength(250)]
        public string? Parameter_Value_Text { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? Parameter_Value_Numeric { get; set; }

        public DateTime? Parameter_Value_Date { get; set; }

        public bool? Parameter_Value_Boolean { get; set; }

        public byte[]? Parameter_Value_Image { get; set; }

        public int Status_ID { get; set; }

        [Required]
        [MaxLength(50)]
        public string Status_Value { get; set; } = string.Empty;

        public int Created_By_ID { get; set; }

        [Required]
        [MaxLength(100)]
        public string Created_By_Name { get; set; } = string.Empty;

        public DateTime Created_Date { get; set; }

        public int? Updated_By_ID { get; set; }

        [MaxLength(100)]
        public string? Updated_By_Name { get; set; }

        public DateTime? Updated_Date { get; set; }
    }

}
