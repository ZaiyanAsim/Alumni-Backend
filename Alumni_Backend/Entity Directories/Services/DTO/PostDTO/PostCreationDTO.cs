using Alumni_Portal.Infrastructure.Data_Models;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System.ComponentModel.DataAnnotations;

namespace Entity_Directories.Services.DTO
{
    public class PostCreationDTO
    {

        public required MetaData Details { get; set; }
       
        public List<PostMentionsDTO> Mentions { get; set; } = [];

      



    }
       

        
        



    public class PostMentionsDTO
    {
        public required int Mention_ID{ get; set; }

        public string Mention_Type { get; set; } = null!;

        public string Mention_Name { get; set; } = null!;
    }

    

    public class MetaData
    {
        public int? Post_Type_ID { get; set; }

        public string? Post_Type_Value { get; set; }
        public required string Post_Title { get; set; }

        public string? Post_Tags { get; set; }
        public string? Post_Content { get; set; }

        public int? Status_ID { get; set; }


        public string? Status_Value { get; set; }



        public int? Created_By_ID { get; set; }


        public string? Created_By_Name { get; set; }

        public DateTime? Created_Date { get; set; }

        public DateTime Published_Date { get; set; }

    }

}
