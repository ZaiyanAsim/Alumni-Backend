using System.Security.Cryptography;

namespace Alumni_Portal.Auth
{
    public static class PasswordHasher
    {
        public static string Hash(string plainText)
        {
            byte[] salt = RandomNumberGenerator.GetBytes(16);
            byte[] hash = Rfc2898DeriveBytes.Pbkdf2(
                password: plainText,
                salt: salt,
                iterations: 600_000,
                hashAlgorithm: HashAlgorithmName.SHA512,
                outputLength: 64
            );
            return $"{Convert.ToBase64String(salt)}:{Convert.ToBase64String(hash)}";
        }

        public static bool Verify(string plainText, string storedHash)
        {
            var parts = storedHash.Split(':');
            if (parts.Length != 2) return false;

            byte[] salt         = Convert.FromBase64String(parts[0]);
            byte[] expectedHash = Convert.FromBase64String(parts[1]);
            byte[] actualHash   = Rfc2898DeriveBytes.Pbkdf2(
                password: plainText,
                salt: salt,
                iterations: 600_000,
                hashAlgorithm: HashAlgorithmName.SHA512,
                outputLength: 64
            );
            return CryptographicOperations.FixedTimeEquals(actualHash, expectedHash);
        }
    }
}
