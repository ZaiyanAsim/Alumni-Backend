using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace Entity_Directories.Services.DTO
{
    public class postDirectoryDTO
    {
        public required  int Post_ID { get; init; }

        public required string Post_Type { get; init; }

        public required string Post_Title { get; init; }

        public string? Post_Content { get; init; }

        public string ? Post_Association { get; init; }

        public string ? Post_Tags { get; init; }

        public string? Post_Status { get; init; }

        public string? Created_By_Name { get; init; }

        public DateTime? Created_Date { get; init; }

        public DateTime? Published_Date { get; init; }


    }
}
