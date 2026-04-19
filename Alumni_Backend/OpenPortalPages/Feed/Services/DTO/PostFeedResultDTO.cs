namespace Alumni_Portal.OpenPortalPages.MainPage.Services.DTO
{


    public class PostMediaDTO
    {
        public int Post_Media_ID { get; set; }
        public string Media_Title { get; set; } = null!;
        public string? Media_Description { get; set; }
        public string? Media_File_Location { get; set; }
        public string? Media_File_Name { get; set; }
    }
      

     

        



    public class PostCreationDTO
    {
        public required PostFeedItemDTO Post { get; set; }
        public List<PostMentionsDTO> Mentions { get; set; } = [];
        public List<PostMediaDTO> Media { get; set; } = [];
    }

    

    public class PostMentionsDTO
    {
        public required int Mention_ID { get; set; }
        public string Mention_Type { get; set; } = null!;
        public string Mention_Name { get; set; } = null!;
    }



    public class PostFeedQueryDTO
    {
        public DateTime? CursorDate { get; set; }
        public int? CursorPostId { get; set; }
        public int PageSize { get; set; } = 20;
        public int? Post_Type_Id { get; set; }
    }





    public class PostFeedItemDTO
    {
        public int Post_ID { get; set; }
        public string Post_Type_Value { get; set; } = null!;
        public string Post_Title { get; set; } = null!;
        public string? Post_Tags { get; set; }
        public string? Post_Content { get; set; }
        public DateTime Published_Date { get; set; }
        public List<PostMentionsDTO> Mentions { get; set; } = [];
        public List<PostMediaDTO> Media { get; set; } = [];

        
    }



    public class PostFeedResultDTO
    {
        public List<PostFeedItemDTO> Posts { get; set; } = [];
        public DateTime? NextCursorDate { get; set; }
        public int? NextCursorPostId { get; set; }
        public bool HasMore => NextCursorDate is not null;
    }
}