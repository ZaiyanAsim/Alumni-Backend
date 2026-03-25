namespace Entity_Directories.Services.DTO
{
    public class PostFilters
    {
        public List<int?> Types { get; set; } = [];

        public List<int?> Association_Types{ get; set; } = [];

        

    }

    public class PostTaggingSearchFilters
    {
        public string Search_Term { get; set; } = string.Empty;
        public string? Tag_Type { get; set; } = string.Empty;
    }
}
