using System.Security.Cryptography;

namespace GBM.PasswordHasher
{
    public class PasswordHasher
    {
        private const int SaltSize = 16;
        private const int HashSize = 20;
        private const int Iterations = 10000;

        public static string HashPassword(string password)
        {
            // Generate a random salt
            byte[] salt = new byte[SaltSize];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            // Hash the password and the salt
            byte[] hash = HashPasswordWithSalt(password, salt, Iterations, HashSize);

            // Combine the salt and hash into a single string
            byte[] saltAndHash = new byte[SaltSize + HashSize];
            Array.Copy(salt, 0, saltAndHash, 0, SaltSize);
            Array.Copy(hash, 0, saltAndHash, SaltSize, HashSize);
            string saltAndHashString = Convert.ToBase64String(saltAndHash);

            return saltAndHashString;
        }

        public static bool VerifyPasswordHash(string password, string saltAndHashString)
        {
            // Decode the salt and hash from the combined string
            byte[] saltAndHash = Convert.FromBase64String(saltAndHashString);
            byte[] salt = new byte[SaltSize];
            byte[] storedHash = new byte[HashSize];
            Array.Copy(saltAndHash, 0, salt, 0, SaltSize);
            Array.Copy(saltAndHash, SaltSize, storedHash, 0, HashSize);

            // Hash the password with the stored salt and iterations
            byte[] hashToCheck = HashPasswordWithSalt(password, salt, Iterations, HashSize);

            // Compare the hashes
            return hashToCheck.SequenceEqual(storedHash);
        }

        private static byte[] HashPasswordWithSalt(string password, byte[] salt, int iterations, int outputBytes)
        {
            using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, iterations))
            {
                return pbkdf2.GetBytes(outputBytes);
            }
        }
    }
}
