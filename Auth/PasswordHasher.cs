using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AuthService
{
    public static class PasswordHasher
    {
        private static byte[] GenerateSalt()
        {
            byte[] salt = new byte[16];
            RandomNumberGenerator rng = RandomNumberGenerator.Create();
            using (rng)
            {
                rng.GetBytes(salt);
            }
            return salt;
        }
        private static byte[] GenerateHash(string password, byte[] salt)
        {
            byte[] hash = KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 32
                );
            return hash;
        }
        public static (string,string) HashPassword(string password)
        {
            byte[] salt = GenerateSalt();
            byte[] hash = GenerateHash(password, salt);
            byte[] hashBytes = new byte[48];
            Array.Copy(salt, 0, hashBytes, 0, 16);
            Array.Copy(hash, 0, hashBytes, 16, 32);
            return (Convert.ToBase64String(hashBytes),Convert.ToBase64String(salt));

        }
        public static bool VerifyPassword(string password, string storedHash, string storedSalt)
        {
            byte[] hashBytes = Convert.FromBase64String(storedHash);
            byte[] salt = Convert.FromBase64String(storedSalt);
            byte[] storedHashPart = new byte[32];
            Array.Copy(hashBytes,16,storedHashPart, 0,32);
            byte[] hash = GenerateHash(password, salt);
            for (int i = 0; i < 32; i++)
            {
                if (storedHashPart[i] != hash[i])
                {
                    return false;
                }
            }
            return true;
        }
    }
}
