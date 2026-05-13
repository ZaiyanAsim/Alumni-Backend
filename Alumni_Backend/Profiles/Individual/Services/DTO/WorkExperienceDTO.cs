namespace Alumni_Portal.Profiles.Individual.Services.DTO
{
    public class IndividualWorkExperienceDto
    {
        public int Individual_ID { get; set; }

        public string? Company_Name { get; set; }

        public string? Job_Title { get; set; }

        public string? Department { get; set; }

        public string? Industry { get; set; }

        public string? Employment_Type { get; set; }

        public DateTime? Start_Date { get; set; }

        public DateTime? End_Date { get; set; }

       public bool Is_Current { get; set; }

        public string? Location { get; set; }

        public string? Description { get; set; }

        public string? Skills { get; set; }

    }

}
