using System;
using System.Linq;
using System.Security.Cryptography;

namespace BL.Security
{
    public static class SecurityHelper
    {
        private const int PBKDF2IterCount = 100000;
        private const int PBKDF2SubkeyLength = 160 / 8;
        private const int saltSize = 128 / 8;

        public static (string hash, string salt) CreateHash(string password)
        {
            using (var deriveBytes = new Rfc2898DeriveBytes(password, saltSize, PBKDF2IterCount))
            {
                var salt = deriveBytes.Salt;
                var subkey = deriveBytes.GetBytes(PBKDF2SubkeyLength);

                return (Convert.ToBase64String(subkey), Convert.ToBase64String(salt));
            }
        }

        public static byte[] CreateHash(string salt, string password)
        {
            var saltBytes = Convert.FromBase64String(salt);
            using (var deriveBytes = new Rfc2898DeriveBytes(password, saltBytes, PBKDF2IterCount))
            {
                return deriveBytes.GetBytes(PBKDF2SubkeyLength);
            }
        }

        public static bool VerifyHashedPassword(string hashedPassword, string salt, string password)
        {
            var hashedPasswordBytes = Convert.FromBase64String(hashedPassword);
            return hashedPasswordBytes.SequenceEqual(CreateHash(salt, password));
        }
    }
}
