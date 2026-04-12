namespace Entity_Directories.Services.DTO
{
    public class MentionsResultDTO
    {
       public List<MentionDTO> IndividualMentions { get; set; } = [];
       public List<MentionDTO> ProjectMentions { get; set; } = [];    
    }

    public class MentionDTO
    {
        public int Id { get; set; }
        public string? Name { get; set; } = string.Empty;
        public string? Type { get; set; } = string.Empty;
    }
}
