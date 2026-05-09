using Alumni_Portal.Auth.Configuration;
using Alumni_Portal.Auth.Services.DTO;
using Alumni_Portal.Auth;
using Alumni_Portal.Infrastructure.Data_Models;


using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;


using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Alumni_Portal.Auth.Services
{
  
    public class LoginResult
    {
        public bool Success { get; private set; }
        public string? ErrorMessage { get; private set; }
        public LoginResponseDTO? Response { get; private set; }

        public static LoginResult Ok(LoginResponseDTO response) =>
            new() { Success = true, Response = response };

        public static LoginResult Fail(string message) =>
            new() { Success = false, ErrorMessage = message };
    }

    

    public class LoginMainService 
    {
        private readonly AuthRepository _repo;
        private readonly JwtSettings _jwtSettings;

        private readonly ILogger<LoginMainService> _logger;

        public LoginMainService(AuthRepository repo, IOptions<JwtSettings> jwtOptions, ILogger<LoginMainService> logger)
        {
            _repo = repo;
            _jwtSettings = jwtOptions.Value;
            _logger = logger;
        }

        public async Task<LoginResult> LoginAsync(LoginRequestDTO request)
        {
           
            var individual = await _repo.GetByEmailAsync(request.Email);

            if (individual is null)
                return LoginResult.Fail("Invalid email or password.");


            if (string.IsNullOrWhiteSpace(individual.password_hash))
                return LoginResult.Fail(
                    "This account has not completed registration. Please register first."
                );

        
            bool passwordValid = VerifyPassword(request.Password, individual.password_hash);


            if (!passwordValid)
                return LoginResult.Fail("Invalid email or password.");

           
            var (token, expiresAt) = GenerateJwtToken(individual);

            var response = new LoginResponseDTO
            {
                AccessToken = token,
                ExpiresAt = expiresAt,
                IndividualId = individual.Individual_ID,
                Email = individual.Individual_Email ?? string.Empty,
                IndividualName = individual.Individual_Name,
                InstitutionId = individual.Individual_Institution_ID,
                IsAlumni = individual.Individual_Is_Alumni,
                Role = individual.Individual_Type_Value
            };
           
            return LoginResult.Ok(response);
        }

        private (string token, DateTime expiresAt) GenerateJwtToken(Individuals individual)
        {

            _logger.LogInformation(_jwtSettings.SecretKey);

            var expiresAt = DateTime.UtcNow.AddMinutes(_jwtSettings.AccessTokenExpiryMinutes);
            var signingKey = new SymmetricSecurityKey(
                                 Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));

            var claims = new List<Claim>
    {
       
        new(JwtRegisteredClaimNames.Sub,   individual.Individual_ID.ToString()),
        new(JwtRegisteredClaimNames.Email, individual.Individual_Email ?? string.Empty),
        new(JwtRegisteredClaimNames.Jti,   Guid.NewGuid().ToString()),
        new(JwtRegisteredClaimNames.Iat,   DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString()),

       
        new("institution_id",   individual.Individual_Institution_ID),
        new("is_alumni",        individual.Individual_Is_Alumni.ToString().ToLower()),
        new("role",             individual.Individual_Type_Value),
        new("individual_name",  individual.Individual_Name)
    };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = expiresAt,
                Issuer = _jwtSettings.Issuer,
                Audience = _jwtSettings.Audience,
                SigningCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256)
            };

            var handler = new JwtSecurityTokenHandler();
            var token = handler.CreateToken(tokenDescriptor);

            return (handler.WriteToken(token), expiresAt);
        }

        private static bool VerifyPassword(string plainTextPassword, string storedHash)
        {
            var parts = storedHash.Split(':');
            if (parts.Length != 2) return false;

            byte[] salt = Convert.FromBase64String(parts[0]);
            byte[] expectedHash = Convert.FromBase64String(parts[1]);

            byte[] actualHash = Rfc2898DeriveBytes.Pbkdf2(
                password: plainTextPassword,
                salt: salt,
                iterations: 600_000,                  
                hashAlgorithm: HashAlgorithmName.SHA512,
                outputLength: 64
            );

            return CryptographicOperations.FixedTimeEquals(actualHash, expectedHash);
        }

        public async Task EnterDummyPasswords(string password, string id)
        {
            
            
           
            await _repo.EnterDummyPasswords(HashPassword(password), id);
        }

        private static string HashPassword(string plainTextPassword)
        {
            byte[] salt = RandomNumberGenerator.GetBytes(16);

            byte[] hash = Rfc2898DeriveBytes.Pbkdf2(
                password: plainTextPassword,
                salt: salt,
                iterations: 600_000,
                hashAlgorithm: HashAlgorithmName.SHA512,
                outputLength: 64
            );

            return $"{Convert.ToBase64String(salt)}:{Convert.ToBase64String(hash)}";
        }
    }
}