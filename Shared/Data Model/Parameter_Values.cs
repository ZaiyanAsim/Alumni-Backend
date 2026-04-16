using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTO
{

    public class Parameter_Values
    {
        [Key]
        public int Parameter_Value_ID { get; set; }
        public int Client_ID { get; set; }

        [MaxLength(100)]
        public  string Client_Reference_Key { get; set; }

        public int Parameter_ID { get; set; }

        [MaxLength(250)]
        public  string Parameter_Reference_Key { get; set; }

        [MaxLength(250)]
        public  string Parameter_Value_Name { get; set; }

        [MaxLength(100)]
        public string Parameter_Value_Reference_Key { get; set; }

        [MaxLength(250)]
        public string Parameter_Value_Description { get; set; }

        public int? Parameter_Value_Parent_ID { get; set; }

        public bool Parameter_Value_Is_Header { get; set; }

        public int Parameter_Value_Sequence_Number { get; set; }

        public int Parameter_Value_Type_ID { get; set; }

        [MaxLength(50)]
        public string Parameter_Value_Type_Value { get; set; }

        [MaxLength(250)]
        public string Parameter_Value_Text { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? Parameter_Value_Numeric { get; set; }

        public DateTime? Parameter_Value_Date { get; set; }

        public bool? Parameter_Value_Boolean { get; set; }

        public byte[] Parameter_Value_Image { get; set; }

        public int Status_ID { get; set; }

        [MaxLength(50)]
        public string Status_Value { get; set; }

        public int Created_By_ID { get; set; }

        [MaxLength(100)]
        public string Created_By_Name { get; set; }

        public DateTime Created_Date { get; set; }

        public int? Updated_By_ID { get; set; }

        [MaxLength(100)]
        public string Updated_By_Name { get; set; }

        public DateTime? Updated_Date { get; set; }
    }
}
