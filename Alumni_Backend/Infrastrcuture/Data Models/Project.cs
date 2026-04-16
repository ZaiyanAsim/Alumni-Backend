using Alumni_Portal.Infrastrcuture.Data_Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Numerics;



namespace Alumni_Portal.Infrastructure.Data_Models;

public class Projects
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

    public string? Logo_Url { get; init; }

    public string? Video_Url { get; init; }
    [Required]
    [MaxLength(100)]
    public string Project_Name { get; set; } = string.Empty;

    [Required]
    public int Project_Year { get; set; }

    [MaxLength(200)]
    public string? Project_Industries { get; set; }


    [MaxLength(500)]
    public string? Project_Description { get; set; }
    public bool Is_Mentored { get; set; } = false;

    public bool Is_Sponsored { get; set; } = false;

    public bool Is_Mentorship_Available { get; set; } = false;

    public bool Is_Sponsorship_Available { get; set; } = false;

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

    public virtual ICollection<Project_Individuals> Project_Individuals { get; set; } = new List<Project_Individuals>();

    public virtual ICollection<Project_Industry> Project_Industry { get; set; }= new List<Project_Industry>();
}


[Table("Project_Attachments")]
public class Project_Attachments
{
    [Key]
    [Column("Project_Attachment_ID")]
    public int Project_Attachment_ID { get; set; }
    public Projects Project { get; set; } = null!;
    [Required]
    [ForeignKey("Project_ID")]
    public int Project_ID { get; set; }

    [Column("Attachment_Date")]
    public DateTime Attachment_Date { get; set; }

    [Column("Attachment_Title")]
    [MaxLength(100)]
    public string Attachment_Title { get; set; }

    [Column("Attachment_Description")]
    public string? Attachment_Description { get; set; }

    [Column("Attachment_File_Location")]
    [MaxLength(500)]
    public string? Attachment_File_Location { get; set; }

    [Column("Attachment_File_Name")]
    [MaxLength(250)]
    public string? Attachment_File_Name { get; set; }

    public long? Attachment_Size { get; set; }

    [Column("Progress_ID")]
    public int Progress_ID { get; set; }

    [Column("Progress_Value")]
    [MaxLength(50)]
    public string Progress_Value { get; set; }

    [Column("Status_ID")]
    public int Status_ID { get; set; }

    [Column("Status_Value")]
    [MaxLength(50)]
    public string Status_Value { get; set; }

    [Column("Created_By_ID")]
    public int Created_By_ID { get; set; }

    [Column("Created_By_Name")]
    [MaxLength(100)]
    public string Created_By_Name { get; set; }

    [Column("Created_Date")]
    public DateTime Created_Date { get; set; }

    [Column("Updated_By_ID")]
    public int? Updated_By_ID { get; set; }

    [Column("Updated_By_Name")]
    [MaxLength(100)]
    public string? Updated_By_Name { get; set; }

    [Column("Updated_Date")]
    public DateTime? Updated_Date { get; set; }
}


public class Project_Media
{
    [Key]
    public int Project_Media_ID { get; set; }

    public Projects Project { get; set; } = null!;
    [Required]
    [ForeignKey("Project_ID")]
    public int Project_ID { get; set; }


    public DateTime Media_Date { get; set; }
    public string Media_Title { get; set; }
    public string Media_Description { get; set; }

    public string Media_File_Location { get; set; }
    public string Media_File_Name { get; set; }

    public int Progress_ID { get; set; }
    public string Progress_Value { get; set; }

    public int Status_ID { get; set; }
    public string Status_Value { get; set; }

    public int Created_By_ID { get; set; }
    public string Created_By_Name { get; set; }
    public DateTime Created_Date { get; set; }

    public int? Updated_By_ID { get; set; }
    public string Updated_By_Name { get; set; }
    public DateTime? Updated_Date { get; set; }
}




[Table("Project_Results")]
public class Project_Results
{

    [Key]
    public int Project_Result_ID { get; set; }
    public Projects Project { get; set; } = null!;
    [Required]
    [ForeignKey("Project_ID")]
    public int Project_ID { get; set; }


    public required string Result_Title { get; set; }
    public string? Result_Description { get; set; }

    public required int  Result_Type_ID { get; set; }
    public required string Result_Type_Value { get; set; }

    public string? Result_Image_Url { get; set; }

    public int? Result_Seq_Number { get; set; }
    public string? Result_Metric_Value { get; set; }
    public string? Result_Metric_Label { get; set; }

    public string? Result_Link { get; set; }

    public string? Result_Tags { get; set; }

    public int Created_By_ID { get; set; }

    public int Created_By_Name { get; set; }


}


public class Project_Delivarables
{

    [Key]
    public int Project_Deliverables_ID { get; set; }
    public Projects Project { get; set; } = null!;
    [Required]
    [ForeignKey("Project_ID")]
    public int Project_ID { get; set; }


    public required string Deliverable_Title { get; set; }
    public string? Deliverable_Description { get; set; }

    public required int Deliverable_Status_ID { get; set; }
    public required string Deliverable_Status_Value { get; set; }

    public required int Deliverable_Category_ID { get; set; }
    public required string Deliverable_Category_Value { get; set; }


    public int Created_By_ID { get; set; }

    public int Created_By_Name { get; set; }

    public DateTime Date { get; set; }
   


}
