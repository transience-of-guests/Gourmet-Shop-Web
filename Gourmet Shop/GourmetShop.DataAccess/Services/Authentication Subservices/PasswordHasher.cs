using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace GourmetShop.DataAccess.Services
{
    public static class PasswordHasher
    {
        private static string PBKDF2_Password(string password, byte[] salt, int iterations = 4)
        {
            using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, iterations, HashAlgorithmName.SHA256))
            {
                byte[] hash = pbkdf2.GetBytes(32);
                byte[] hashBytes = new byte[52]; // 16(salt) + 32(hash) + 4(iteration count)

                Array.Copy(salt, 0, hashBytes, 0, 16);
                Array.Copy(hash, 0, hashBytes, 16, 32);

                string base64 = Convert.ToBase64String(hashBytes);
                // Converting to UTF-16 so that it works with NVarchar(255) in the database as the latter uses UTF-16 encoding
                return Encoding.Unicode.GetString(Encoding.Unicode.GetBytes(base64));
            }
        }
        public static string HashPassword(string password, int iterations = 4)
        {
            byte[] salt;

            using (var rnrg = new RNGCryptoServiceProvider())
            {
                rnrg.GetBytes(salt = new byte[16]);
            }

            return PBKDF2_Password(password, salt, iterations);
        }

        public static string HashPassword(string password, byte[] salt, int iterations = 4)
        {
            return PBKDF2_Password(password, salt, iterations);
        }

        private static byte[] GetSalt(string hashedPassword)
        {
            byte[] hashBytes = Convert.FromBase64String(Encoding.Unicode.GetString(Encoding.Unicode.GetBytes(hashedPassword)));
            byte[] salt = new byte[16];
            Array.Copy(hashBytes, 0, salt, 0, 16);

            return salt;
        }

        // FIXME: Hashing differently each time due to generating a new salt each time
        public static bool VerifyPassword(string inputPassword, string hashedPassword)
        {
            return HashPassword(inputPassword, GetSalt(hashedPassword)).Equals(hashedPassword);
        }
    }
}
