namespace Alumni_Portal.Infrastrcuture.Data_Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Events
    {
        [Key]
        public int Event_ID { get; set; }

       
        public int? Event_Type_ID { get; set; }


        public string ?Event_Type_Value { get; set; }

        public string Event_Name { get; set; } = null!;

        public string? Event_Description { get; set; }

        public DateTime Event_Date { get; set; }

        public string? Event_Status { get; set; }

        public string? Event_Logo_URL { get; set; }
        public string? Event_Image_URL { get; set; }

        
        public DateTime Created_At { get; set; } = DateTime.UtcNow;

        public DateTime? Updated_At { get; set; }
    }

}
