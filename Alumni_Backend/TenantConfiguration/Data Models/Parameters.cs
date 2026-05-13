using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Alumni_Portal.TenantConfiguration.Data_Models
{
    [Table("Parameters", Schema = "dbo")]
    public class Parameters
    {
        

            [Key]     
            public int Parameter_ID { get; init; }
            public int Client_ID { get; init; }
            public string Client_Reference_Key { get; init; } = string.Empty;

            public string Parameter_Name { get; init; } = string.Empty;
            public string Parameter_Reference_Key { get; init; } = string.Empty;
            public string Parameter_Description { get; init; } = string.Empty;

            public string Parameter_Type { get; init; } = string.Empty;
            public string Parameter_Type_Reference_Key { get; init; } = string.Empty;

            public int Status_ID { get; init; }
            public string Status_Value { get; init; } = string.Empty;

            public int Created_By_ID { get; init; }
            public string Created_By_Name { get; init; } = string.Empty;
            public DateTime Created_Date { get; init; }

            public int? Updated_By_ID { get; init; }
            public string? Updated_By_Name { get; init; }
            public DateTime? Updated_Date { get; init; }
        }

    }

