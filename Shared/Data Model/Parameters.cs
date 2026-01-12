using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Data_Model
{
    public class Parameters
    {
        [Key]
        public int Parameter_ID { get; set; }
        public int Client_ID { get; set; }

        [MaxLength(100)]
        public string Client_Reference_Key { get; set; }

        [MaxLength(100)]
        public string Parameter_Name { get; set; }

        [MaxLength(100)]
        public string Parameter_Reference_Key { get; set; }

        [MaxLength(250)]
        public string Parameter_Description { get; set; }

        [MaxLength(100)]
        public string Parameter_Type { get; set; }

        [MaxLength(100)]
        public string Parameter_Type_Reference_Key { get; set; }

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
