namespace Entity_Directories.Services.DTO
{
    public class CreateEventDTO
    {
        public required string Event_Name { get; set; }
        public DateTime Event_Date { get; set; }
        public string? Event_Description { get; set; }
        public int? Event_Type_ID { get; set; }
        public string? Event_Type_Value { get; set; }
        public string? Event_Status { get; set; }
        public string? Event_Logo_URL { get; set; }
        public string? Event_Image_URL { get; set; }
    }

    public class EventDirectoryDTO
    {
        public int Event_ID { get; init; }
        public string Event_Name { get; init; } = null!;
        public DateTime Event_Date { get; init; }
        public string? Event_Type_Value { get; init; }
        public string? Event_Status { get; init; }
        public string? Event_Description { get; init; }
        public string? Event_Logo_URL { get; init; }
        public string? Event_Image_URL { get; init; }
        public DateTime Created_At { get; init; }
    }

    public class UpcomingEventDTO
    {
        public int Event_ID { get; init; }
        public string Event_Name { get; init; } = null!;
        public DateTime Event_Date { get; init; }
        public string? Event_Description { get; init; }
        public string? Event_Logo_URL { get; init; }
        public string? Event_Image_URL { get; init; }
        public string? Event_Type_Value { get; init; }
    }
}
