using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;

namespace RestAPI.Security
{
    /// <summary>
    /// Helper class for JTW Token
    /// </summary>
    public static class JwtTokenHelper
    {
        private static string secret;

        /// <summary>
        /// Symmetric Key
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown when <see cref="secret"/> is null</exception>
        public static string Secret
        {
            get { return secret ?? throw new InvalidOperationException(); }
            set { secret = value; }
        }

        /// <summary>
        /// Validation parameters
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown when <see cref="Secret"/> is null</exception>
        public static TokenValidationParameters ValidationParameters => new TokenValidationParameters()
        {
            IssuerSigningKey = new SymmetricSecurityKey(Convert.FromBase64String(Secret)),
            ClockSkew = new TimeSpan(0),
            RequireExpirationTime = true,
            ValidateLifetime = true,
            ValidateAudience = false,
            ValidateIssuer = false
        };

        /// <summary>
        /// Generates token for user
        /// </summary>
        /// <param name="email">User's email</param>
        /// <param name="expireSeconds">Token expiration time in seconds</param>
        /// <returns>Generated token</returns>
        /// <exception cref="InvalidOperationException">Thrown when <see cref="Secret"/> is null</exception>
        public static string GenerateToken(string email, int expireSeconds = 30 * 60)
        {
            if (string.IsNullOrEmpty(email))
                return null;

            var symmetricKey = Convert.FromBase64String(Secret);
            var tokenHandler = new JwtSecurityTokenHandler();

            var now = DateTime.UtcNow;
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Email, email) }),
                Expires = now.AddSeconds(expireSeconds),
                IssuedAt = DateTime.Now,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(symmetricKey), SecurityAlgorithms.HmacSha256Signature)
            };

            return tokenHandler.WriteToken(tokenHandler.CreateToken(tokenDescriptor));
        }

        /// <summary>
        /// Verifies provided token
        /// </summary>
        /// <param name="token">Token to be verified</param>
        /// <returns>User email obtained from token claims</returns>
        /// <exception cref="InvalidOperationException">Thrown when <see cref="Secret"/> is null</exception>
        public static string VerifyToken(string token)
        {
            if (string.IsNullOrEmpty(token))
                return null;

            var principal = new JwtSecurityTokenHandler().ValidateToken(token, ValidationParameters, out var _);
            return principal.Identities.FirstOrDefault()?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
        }
    }
}
