using Microsoft.IdentityModel.Tokens;
using RestAPI.Security;
using System.Threading;
using Xunit;

namespace Tests.UI
{
    public class JwtTokenTests
    {
        private const string email = "test@mail.sk";

        [Fact]
        public void TestValidToken()
        {
            var token = JwtTokenHelper.GenerateToken(email);
            Assert.Equal(email, JwtTokenHelper.VerifyToken(token));
        }

        [Fact]
        public void TestExpiredToken()
        {
            var token = JwtTokenHelper.GenerateToken(email, 1);
            Thread.Sleep(2000);

            Assert.Throws<SecurityTokenExpiredException>(() => JwtTokenHelper.VerifyToken(token));
        }
    }
}
