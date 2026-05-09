namespace Alumni_Portal.Engagement.Services.DTO
{
    public class RequestRejectionDTO
    {
        public int Request_ID { get; set; }
        public string? Individual_Name { get; set; }
        public required string Individual_Email { get; set; }
    
        public string? Request_Type_Value { get; set; }







        public string? Project_Academic_ID { get; set; }


        public string? Project_Name { get; set; }

 
        public string? Rejection_Reason { get; set; }
    }
}