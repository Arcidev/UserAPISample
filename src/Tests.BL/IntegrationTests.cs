using BL.Configuration;
using BL.DTO;
using BL.DTO.User;
using BL.Exceptions;
using BL.Facades;
using DAL.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Tests.BL
{
    public class IntegrationTests
    {
        private readonly IServiceProvider services;
        private const string password = "123456";
        private const string email = "mail@test.sk";
        private const string name = "User Name";
        private const string phone = "+421 666 666 666";
        private static UserCreateDTO User => new UserCreateDTO()
        {
            Email = email,
            Name = name,
            Telephones = new List<PhoneDTO>() { new PhoneDTO() { Number = phone } },
            Password = password,
            Token = password
        };

        public IntegrationTests()
        {
            AutoMapperInstaller.Init();

            services = new ServiceCollection()
                .AddDbContext<AppDbContext>(options => options.UseInMemoryDatabase("TestDB"), ServiceLifetime.Transient, ServiceLifetime.Transient)
                .AddTransient<Func<DbContext>>(provider => () => provider.GetService<AppDbContext>())
                .ConfigureServices()
                .ConfigureFacades()
                .BuildServiceProvider();
        }

        [Fact]
        public async Task TestAddUser()
        {
            var userFacade = services.GetService<UserFacade>();
            var signedUser = await userFacade.AddUserAsync(User);

            Assert.NotNull(signedUser);
            Assert.NotEqual(Guid.Empty, signedUser.Id);
            Assert.NotEqual(default(DateTime), signedUser.LastLoginOn);
            Assert.NotEqual(default(DateTime), signedUser.CreatedOn);
        }

        [Fact]
        public async Task TestSignInUser()
        {
            var user = User;
            var userFacade = services.GetService<UserFacade>();
            await userFacade.AddUserAsync(user);

            var signedUser = await userFacade.SignInUser(new UserCredentialsDTO() { Email = user.Email, Password = user.Password, Token = user.Token });
            Assert.NotNull(signedUser);
            Assert.NotEqual(Guid.Empty, signedUser.Id);
            Assert.NotEqual(default(DateTime), signedUser.LastLoginOn);
            Assert.NotEqual(default(DateTime), signedUser.CreatedOn);
        }

        [Fact]
        public async Task TestInvalidTokenException()
        {
            var userFacade = services.GetService<UserFacade>();
            var user = await userFacade.AddUserAsync(User);
            
            await Assert.ThrowsAsync<BLException>(() => userFacade.VerifyAndGetUser(user.Id, string.Empty));
            await Assert.ThrowsAsync<BLException>(() => userFacade.VerifyAndGetUser(Guid.Empty, password));
        }

        [Fact]
        public async Task TestInvalidCredentials()
        {
            var user = User;
            var userFacade = services.GetService<UserFacade>();
            await userFacade.AddUserAsync(user);

            await Assert.ThrowsAsync<BLException>(() => userFacade.SignInUser(new UserCredentialsDTO()));
            await Assert.ThrowsAsync<BLException>(() => userFacade.SignInUser(new UserCredentialsDTO() { Email = user.Email, Password = $"{password}{password}" }));
        }

        [Fact]
        public async Task TestAddAndVerifyUser()
        {
            var user = User;
            var userFacade = services.GetService<UserFacade>();
            var signedUser = await userFacade.AddUserAsync(user);

            var userDto = await userFacade.VerifyAndGetUser(signedUser.Id, password);
            Assert.NotNull(userDto);
            Assert.Equal(signedUser.Id, userDto.Id);
            Assert.Equal(signedUser.LastLoginOn, userDto.LastLoginOn);
            Assert.Equal(signedUser.CreatedOn, userDto.CreatedOn);
            Assert.Equal(signedUser.LastUpdatedOn, userDto.LastUpdatedOn);
            Assert.Equal(user.Email, userDto.Email);
            Assert.Equal(user.Name, userDto.Name);
            Assert.NotEmpty(userDto.Telephones);
            Assert.Equal(user.Telephones[0].Number, userDto.Telephones[0].Number);
        }
    }
}
