

namespace Alumni_Portal.OpenPortalPages.MainPage.Services.DTO
{
    public class BannerPostDTO
    {
        
            public int Post_ID { get; set; }
            public string Post_Type_Value { get; set; } = null!;
            public string Post_Title { get; set; } = null!;
            public string? Post_Content { get; set; }
            public DateTime Published_Date { get; set; }
            //public List<PostMentionsDTO> Mentions { get; set; } = [];
            public string ? Media_File_Location { get; set; }



    }
    }

