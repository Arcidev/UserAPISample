using BL.Configuration;
using BL.DTO.User;
using BL.Exceptions;
using BL.Facades;
using BL.Repositories;
using BL.Security;
using DAL.Entities;
using Moq;
using Riganti.Utils.Infrastructure.Core;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Tests.BL
{
    public class UserTests
    {
        private const string password = "123456";
        private static readonly Guid guid = Guid.NewGuid();

        public UserTests()
        {
            AutoMapperInstaller.Init();
        }

        [Fact]
        public async Task TestAddUser()
        {
            var mock = new Mock<IUserRepository>();
            mock.Setup(x => x.Insert(It.IsNotNull<User>())).Callback((User user) => user.Id = guid);

            var userFacade = new UserFacade(() => mock.Object, () => CreateUoWProviderMock());

            var signedUser = await userFacade.AddUserAsync(new UserCreateDTO()
            {
                Password = password,
                Token = password
            });

            Assert.NotNull(signedUser);
            Assert.Equal(guid, signedUser.Id);
            Assert.NotEqual(default, signedUser.LastLoginOn);
            Assert.NotEqual(default, signedUser.CreatedOn);
        }

        [Fact]
        public async Task TestSignInUser()
        {
            var user = CreateUserObject();
            var mock = new Mock<IUserRepository>();
            mock.Setup(x => x.GetByEmailAsync(It.IsAny<string>())).Returns(Task.FromResult(user));

            var userFacade = new UserFacade(() => mock.Object, () => CreateUoWProviderMock());
            var signedUser = await userFacade.SignInUser(new UserCredentialsDTO()
            {
                Email = null,
                Password = password,
                Token = password
            });

            Assert.NotNull(signedUser);
            Assert.Equal(guid, signedUser.Id);
        }

        [Fact]
        public async Task TestVerifyAndGetUser()
        {
            var user = CreateUserObject();
            var mock = new Mock<IUserRepository>();
            mock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<IIncludeDefinition<User>>())).Returns(Task.FromResult(user));

            var userFacade = new UserFacade(() => mock.Object, () => CreateUoWProviderMock());
            var userDto = await userFacade.VerifyAndGetUser(user.Id, password);

            Assert.NotNull(userDto);
            Assert.Equal(guid, userDto.Id);
        }

        [Fact]
        public async Task TestInvalidTokenException()
        {
            var user = CreateUserObject();
            user.TokenHash = user.PasswordSalt;

            var mock = new Mock<IUserRepository>();
            mock.Setup(x => x.GetByIdAsync(It.IsNotIn(Guid.Empty), It.IsAny<IIncludeDefinition<User>>())).Returns(Task.FromResult(user));

            var userFacade = new UserFacade(() => mock.Object, () => CreateUoWProviderMock());
            await Assert.ThrowsAsync<BLException>(() => userFacade.VerifyAndGetUser(user.Id, password));
            await Assert.ThrowsAsync<BLException>(() => userFacade.VerifyAndGetUser(Guid.Empty, password));
        }

        [Fact]
        public async Task TestInvalidCredentials()
        {
            var user = CreateUserObject();
            user.TokenHash = user.PasswordSalt;

            var mock = new Mock<IUserRepository>();
            mock.Setup(x => x.GetByEmailAsync(It.IsNotNull<string>())).Returns(Task.FromResult(user));

            var userFacade = new UserFacade(() => mock.Object, () => CreateUoWProviderMock());
            await Assert.ThrowsAsync<BLException>(() => userFacade.SignInUser(new UserCredentialsDTO()));
            await Assert.ThrowsAsync<BLException>(() => userFacade.SignInUser(new UserCredentialsDTO() { Email = string.Empty, Password = $"{password}{password}" }));
        }

        private static User CreateUserObject()
        {
            var (hash, salt) = SecurityHelper.CreateHash(password);
            return new User()
            {
                PasswordHash = hash,
                PasswordSalt = salt,
                Id = guid,
                TokenHash = hash
            };
        }

        private static IUnitOfWorkProvider CreateUoWProviderMock()
        {
            var mockUow = new Mock<IUnitOfWork>();
            mockUow.Setup(x => x.CommitAsync()).Returns(Task.CompletedTask);
            mockUow.Setup(x => x.Dispose());

            var mock = new Mock<IUnitOfWorkProvider>();
            mock.Setup(x => x.Create()).Returns(mockUow.Object);

            return mock.Object;
        }
    }
}
