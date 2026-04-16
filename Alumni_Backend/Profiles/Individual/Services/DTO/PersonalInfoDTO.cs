namespace Alumni_Portal.Profiles.Individual.Services.DTO
{
    public class PersonalInfoDTO
    {
        public string? Name { get; set; }
        public string? Email { get; set; } 

        public string? InstitutionID { get; set; }

        public bool IsAlumni { get; set; }

        public string ? ContactNumberPrimary { get; set; } = null;

        public string? ContactNumberSecondary { get; set; } = null;

        

    }
}
