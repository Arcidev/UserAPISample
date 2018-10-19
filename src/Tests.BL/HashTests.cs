using BL.Security;
using System;
using Xunit;

namespace Tests.BL
{
    public class HashTests
    {
        private const string password = "123456";

        [Fact]
        public void TestCreateHash()
        {
            var (hash, salt) = SecurityHelper.CreateHash(password);
            Assert.NotNull(hash);
            Assert.NotNull(salt);

            var hashWithSalt = SecurityHelper.CreateHash(salt, password);
            Assert.NotNull(hashWithSalt);
            Assert.Equal<byte>(hashWithSalt, Convert.FromBase64String(hash));
        }

        [Fact]
        public void TestVerifyHash()
        {
            var (hash, salt) = SecurityHelper.CreateHash(password);
            Assert.True(SecurityHelper.VerifyHashedPassword(hash, salt, password));
        }
    }
}
