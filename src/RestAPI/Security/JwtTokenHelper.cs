using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;

namespace RestAPI.Security
{
    public class JwtTokenHelper
    {
        public const string Secret = "DSHLQr6nfgcOPqCBcGJWqHrw+/nULwVCpRiAtTTnrBY=";

        public static TokenValidationParameters ValidationParameters => new TokenValidationParameters()
        {
            IssuerSigningKey = new SymmetricSecurityKey(Convert.FromBase64String(Secret)),
            ClockSkew = new TimeSpan(0),
            RequireExpirationTime = true,
            ValidateLifetime = true,
            ValidateAudience = false,
            ValidateIssuer = false
        };

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

        public static string VerifyToken(string token)
        {
            if (string.IsNullOrEmpty(token))
                return null;

            var principal = new JwtSecurityTokenHandler().ValidateToken(token, ValidationParameters, out var _);
            return principal.Identities.FirstOrDefault().Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email).Value;
        }
    }
}
