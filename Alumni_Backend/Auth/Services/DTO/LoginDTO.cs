using System.ComponentModel.DataAnnotations;

namespace Alumni_Portal.Auth.Services.DTO
{
    public class LoginRequestDTO
    {
   
        public string Email { get; set; } = string.Empty;

   
        public string Password { get; set; } = string.Empty;
    }

    public class LoginResponseDTO
    {
        public string AccessToken { get; set; } = string.Empty;
        public DateTime ExpiresAt { get; set; }

     
        public int IndividualId { get; set; }
        public string Email { get; set; } = string.Empty;
        public string IndividualName { get; set; } = string.Empty;
        public string InstitutionId { get; set; } = string.Empty;
        public bool IsAlumni { get; set; }
        public string Role { get; set; } = string.Empty;
    }
}