using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;




namespace Alumni_Portal.Infrastructure.Data_Models
{
    public class Posts
    {
        [Key]
        public int Post_ID { get; set; }

        [Required]
        public int Client_ID { get; set; }

        [MaxLength(100)]
        public string? Client_Reference_Key { get; set; }

        [Required]
        public int Campus_ID { get; set; }

        [MaxLength(100)]
        public string? Campus_Reference_Key { get; set; }

        public int? Post_Type_ID { get; set; }

        [Required]
        [MaxLength(100)]
        public string Post_Type_Value { get; set; } = null!;

        [Required]
        [MaxLength(100)]
        public string Post_Title { get; set; } = null!;

        public string? Post_Content { get; set; }

        [MaxLength(500)]
        public string? Post_Tags { get; set; }

        public int? Progress_ID { get; set; }

        public int? Post_Association_ID { get; set; }

        public string? Post_Association_Value { get; set; }

        [MaxLength(50)]
        public string? Progress_Value { get; set; }

        public int? Status_ID { get; set; }

        [MaxLength(50)]
        public string? Status_Value { get; set; }

        public int? Created_By_ID { get; set; }

        [MaxLength(100)]
        public string? Created_By_Name { get; set; }

        public DateTime? Created_Date { get; set; }

        public int? Updated_By_ID { get; set; }

        [MaxLength(100)]
        public string? Updated_By_Name { get; set; }

        public DateTime? Updated_Date { get; set; }

        public int? Published_By_ID { get; set; }

        [Required]
        [MaxLength(100)]
        public string Pusbished_By_Name { get; set; } = null!;

        public DateTime Published_Date { get; set; }
    }

    public class Post_Mentions
    {
        [Key]
        public int Post_Mention_ID { get; set; }

        public required int Post_ID { get; set; }

        public required int Mention_ID { get; set; }

        public required string Mention_Name { get; set; }

        public required string Mention_Type { get; set; }
    }

    [Table("Post_Attachments")]
    public class Post_Attachment
    {
        [Key]
        public int PostAttachmentId { get; set; }           // Post_Attachment_ID
        public int PostId { get; set; }                     // Post_ID
        public DateTime AttachmentDate { get; set; }       // Attachment_Date
        public string AttachmentTitle { get; set; }        // Attachment_Title
        public string? AttachmentDescription { get; set; } // Attachment_Description
        public string? AttachmentFileLocation { get; set; }// Attachment_File_Location
        public string? AttachmentFileName { get; set; }    // Attachment_File_Name
        public int ProgressId { get; set; }                // Progress_ID
        public string ProgressValue { get; set; }          // Progress_Value
        public int StatusId { get; set; }                  // Status_ID
        public string StatusValue { get; set; }            // Status_Value
        public int CreatedById { get; set; }               // Created_By_ID
        public string CreatedByName { get; set; }          // Created_By_Name
        public DateTime CreatedDate { get; set; }          // Created_Date
        public int? UpdatedById { get; set; }              // Updated_By_ID
        public string? UpdatedByName { get; set; }         // Updated_By_Name
        public DateTime? UpdatedDate { get; set; }         // Updated_Date
    }

    
        public class Post_Media
        {
            [Key]
            public int Post_Media_ID { get; set; }             // Post_Media_ID
            public int Post_ID { get; set; }                  // Post_ID
            public DateTime Media_Date { get; set; }          // Media_Date
            public string Media_Title { get; set; }           // Media_Title
            public string? Media_Description { get; set; }    // Media_Description
            public string? Media_File_Location { get; set; }   // Media_File_Location
            public string? Media_File_Name { get; set; }       // Media_File_Name
            public int Progress_ID { get; set; }              // Progress_ID
            public string Progress_Value { get; set; }        // Progress_Value
            public int Status_ID { get; set; }                // Status_ID
            public string Status_Value { get; set; }          // Status_Value
            public int Created_By_ID { get; set; }             // Created_By_ID
            public string Created_By_Name { get; set; }        // Created_By_Name
            public DateTime Created_Date { get; set; }        // Created_Date
            public int? Updated_By_ID { get; set; }            // Updated_By_ID
            public string? Updated_By_Name { get; set; }       // Updated_By_Name
            public DateTime? Updated_Date { get; set; }       // Updated_Date
        }
    }



