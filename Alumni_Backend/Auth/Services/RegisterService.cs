//using Alumni_Portal.Auth.Services.DTO;



//using System.Security.Cryptography;


//namespace Alumni_Portal.Auth.Services
//{
//    using Alumni_Portal.API.Auth.DTOs;
//    using Alumni_Portal.API.Auth.Models;
//    using Alumni_Portal.API.Auth.Repositories;
//    using global::Alumni_Portal.Auth.Services.DTO;
//    using System.Security.Cryptography;

//    namespace Alumni_Portal.API.Auth.Services
//    {
//        // ── Result wrapper returned by RegisterAsync ───────────────────
//        public class RegisterResult
//        {
//            public bool Success { get; set; }
//            public string? ErrorMessage { get; set; }
//            public Guid? UserId { get; set; }

//            public static RegisterResult Ok(Guid userId) => new() { Success = true, UserId = userId };
//            public static RegisterResult Fail(string message) => new() { Success = false, ErrorMessage = message };
//        }

//        // ── Interface ──────────────────────────────────────────────────
//        public interface IRegisterService
//        {
//            Task<RegisterResult> RegisterAsync(RegisterRequestDto request);
//        }

//        // ── Implementation ─────────────────────────────────────────────
//        public class RegisterService : IRegisterService
//        {
//            private readonly IRegisterRepository _registerRepository;

//            public RegisterService(IRegisterRepository registerRepository)
//            {
//                _registerRepository = registerRepository;
//            }

//            /// <inheritdoc />
//            public async Task<RegisterResult> RegisterAsync(RegisterRequestDto request)
//            {
//                // 1. Check for duplicate email
//                bool emailExists = await _registerRepository.EmailExistsAsync(request.Email);
//                if (emailExists)
//                    return RegisterResult.Fail("An account with this email already exists.");

//                // 2. Hash the password
//                string passwordHash = HashPassword(request.Password);

//                // 3. Build the domain model
//                var user = new AlumniUser
//                {
//                    Id = Guid.NewGuid(),
//                    UserName = request.UserName,
//                    Email = request.Email,
//                    ContactNo = request.ContactNo,
//                    InstitutionId = request.InstitutionId,
//                    Program = request.Program,
//                    Department = request.Department,
//                    YearOfGraduation = request.YearOfGraduation,
//                    CurrentCountry = request.CurrentCountry,
//                    CurrentCity = request.CurrentCity,
//                    Organization = request.Organization,
//                    Designation = request.Designation,
//                    PasswordHash = passwordHash,
//                    CreatedAt = DateTime.UtcNow
//                };

//                // 4. Persist via repository (details to be implemented later)
//                await _registerRepository.CreateUserAsync(user);

//                return RegisterResult.Ok(user.Id);
//            }

//            // ── Password Hashing ───────────────────────────────────────
//            /// <summary>
//            /// Hashes a plain-text password using PBKDF2 with a random salt.
//            /// Output format: Base64(salt):Base64(hash)
//            /// Algorithm : HMACSHA512 | Iterations : 600_000 | Key length : 64 bytes
//            /// </summary>
//            private static string HashPassword(string plainTextPassword)
//            {
//                // Generate a 16-byte cryptographically random salt
//                byte[] salt = RandomNumberGenerator.GetBytes(16);

//                // Derive the key using PBKDF2-HMACSHA512
//                byte[] hash = Rfc2898DeriveBytes.Pbkdf2(
//                    password: plainTextPassword,
//                    salt: salt,
//                    iterations: 600_000,               // OWASP 2024 recommended minimum
//                    hashAlgorithm: HashAlgorithmName.SHA512,
//                    outputLength: 64
//                );

//                // Encode both parts and combine with a colon separator
//                string saltBase64 = Convert.ToBase64String(salt);
//                string hashBase64 = Convert.ToBase64String(hash);

//                return $"{saltBase64}:{hashBase64}";
//            }

//            /// <summary>
//            /// Verifies a plain-text password against a stored hash produced by <see cref="HashPassword"/>.
//            /// </summary>
//            public static bool VerifyPassword(string plainTextPassword, string storedHash)
//            {
//                var parts = storedHash.Split(':');
//                if (parts.Length != 2) return false;

//                byte[] salt = Convert.FromBase64String(parts[0]);
//                byte[] expectedHash = Convert.FromBase64String(parts[1]);

//                byte[] actualHash = Rfc2898DeriveBytes.Pbkdf2(
//                    password: plainTextPassword,
//                    salt: salt,
//                    iterations: 600_000,
//                    hashAlgorithm: HashAlgorithmName.SHA512,
//                    outputLength: 64
//                );

//                // Constant-time comparison to prevent timing attacks
//                return CryptographicOperations.FixedTimeEquals(actualHash, expectedHash);
//            }
//        }
//    }
//}
