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

        /// <summary>
        /// Creates hash
        /// </summary>
        /// <param name="password">Input from which the hash will be created</param>
        /// <returns>Generated salt and hash from the input</returns>
        public static (string hash, string salt) CreateHash(string password)
        {
            using (var deriveBytes = new Rfc2898DeriveBytes(password, saltSize, PBKDF2IterCount))
            {
                var salt = deriveBytes.Salt;
                var subkey = deriveBytes.GetBytes(PBKDF2SubkeyLength);

                return (Convert.ToBase64String(subkey), Convert.ToBase64String(salt));
            }
        }

        /// <summary>
        /// Creates hash while using passed salt
        /// </summary>
        /// <param name="salt">Salt to be used for creating hash</param>
        /// <param name="password">Input from which the hash will be created</param>
        /// <returns>Generated hash</returns>
        public static byte[] CreateHash(string salt, string password)
        {
            var saltBytes = Convert.FromBase64String(salt);
            using (var deriveBytes = new Rfc2898DeriveBytes(password, saltBytes, PBKDF2IterCount))
            {
                return deriveBytes.GetBytes(PBKDF2SubkeyLength);
            }
        }

        /// <summary>
        /// Verifies hash with password
        /// </summary>
        /// <param name="hashedPassword">Hashed password</param>
        /// <param name="salt">Salt used to create hash</param>
        /// <param name="password">Input from which the hash will be created</param>
        /// <returns>True if hashed password was created from password and salt, otherwise false</returns>
        public static bool VerifyHashedPassword(string hashedPassword, string salt, string password)
        {
            var hashedPasswordBytes = Convert.FromBase64String(hashedPassword);
            return hashedPasswordBytes.SequenceEqual(CreateHash(salt, password));
        }
    }
}
