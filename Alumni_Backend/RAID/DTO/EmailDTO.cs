namespace Alumni_Portal.RAID.DTO
{
    public class EmailDTO
    {
        public ICollection<string> toEmails { get; set; }
        public int clientId { get; set; }
        public string subject { get; set; } = string.Empty;
        public string message { get; set; } = string.Empty;
        public ICollection<string> ccEmails { get; set; }
    }
}

