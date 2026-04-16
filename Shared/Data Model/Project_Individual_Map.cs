using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Data_Model
{
    public class Project_Individuals_Map
    {
       
        public int Project_Individuals_Map_ID { get; set; }

       
        public int Project_ID { get; set; }
        public int Individual_ID { get; set; }

        
        public string? Project_Academic_ID { get; set; }
        public string? Individual_Institution_ID { get; set; }

        
        public string? Role_Type { get; set; }
        public int? Created_By_ID { get; set; }

     
        public string? Created_By_Name { get; set; }

        [Column(TypeName = "datetime2(7)")]
        public DateTime? Created_Date { get; set; }

        public int? Updated_By_ID { get; set; }

        public string? Updated_By_Name { get; set; }

        [Column(TypeName = "datetime2(7)")]
        public DateTime? Updated_Date { get; set; }
        public bool Is_Active { get; set; } = true;
    }
}
