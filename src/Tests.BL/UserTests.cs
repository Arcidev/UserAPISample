using BL.Configuration;
using BL.DTO.User;
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
            SetGetByEmailMock(mock, null);
            mock.Setup(x => x.Insert(It.IsNotNull<User>())).Callback((User user) => user.Id = guid);

            var userFacade = new UserFacade(() => mock.Object, () => CreateUoWProviderMock());

            var signedUser = await userFacade.AddUserAsync(new UserCreateDTO()
            {
                Password = password,
                Token = password
            });

            Assert.NotNull(signedUser);
            Assert.Equal(guid, signedUser.Id);
            Assert.NotEqual(default(DateTime), signedUser.LastLoginOn);
            Assert.NotEqual(default(DateTime), signedUser.CreatedOn);
        }

        [Fact]
        public async Task TestSignInUser()
        {
            var user = CreateUserObject();
            var mock = new Mock<IUserRepository>();
            SetGetByEmailMock(mock, user);

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
            mock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).Returns(Task.FromResult(user));

            var userFacade = new UserFacade(() => mock.Object, () => CreateUoWProviderMock());
            var userDto = await userFacade.VerifyAndGetUser(user.Id, password);

            Assert.NotNull(userDto);
            Assert.Equal(guid, userDto.Id);

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

        private static void SetGetByEmailMock(Mock<IUserRepository> mock, User returnObject)
        {
            mock.Setup(x => x.GetByEmailAsync(It.IsAny<string>())).Returns(Task.FromResult(returnObject));
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
