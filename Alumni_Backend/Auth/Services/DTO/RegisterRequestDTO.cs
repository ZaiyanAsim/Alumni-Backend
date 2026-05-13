using System.ComponentModel.DataAnnotations;

namespace Alumni_Portal.Auth.Services.DTO
{
    public class RegisterRequestDto
    {
        // ── Personal Info ──────────────────────────────────────────
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [MaxLength(150)]
        public string Email { get; set; } = string.Empty;

        [Required]
        [Phone]
        [MaxLength(20)]
        public string ContactNo { get; set; } = string.Empty;

        // ── Academic Info ──────────────────────────────────────────
        [Required]
        public int InstitutionId { get; set; }

        [Required]
        [MaxLength(100)]

       
        public string Program { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]

        
        public string Department { get; set; } = string.Empty;

        [Required]
        [Range(1900, 2100)]
        public int YearOfGraduation { get; set; }

    
        [Required]
        [MaxLength(100)]
        public string CurrentCountry { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string CurrentCity { get; set; } = string.Empty;

    
        [MaxLength(150)]
        public string? CurrentOrganization { get; set; }

        [MaxLength(100)]
        public string? CurrentDesignation { get; set; }

        // ── Credentials ────────────────────────────────────────────
        [Required]
        [MinLength(8)]
        public string Password { get; set; } = string.Empty;
    }
}