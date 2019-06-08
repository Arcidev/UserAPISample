using AutoMapper;
using BL.DTO.User;
using BL.Enums;
using BL.Exceptions;
using BL.Repositories;
using BL.Resources;
using BL.Security;
using DAL.Entities;
using Riganti.Utils.Infrastructure.Core;
using Riganti.Utils.Infrastructure.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace BL.Facades
{
    /// <summary>
    /// Facade for user oriented logic
    /// </summary>
    public class UserFacade : BaseFacade
    {
        private readonly Func<IUserRepository> userRepositoryFunc;
        private readonly IMapper mapper;

        /// <summary>
        /// Creates new instance of User facade
        /// </summary>
        /// <param name="userRepositoryFunc">Functor of repository allowing operations on <see cref="User"/> entity</param>
        /// <param name="uowFunc">Functor for instantiating <see cref="IUnitOfWorkProvider"/></param>
        /// <param name="mapper"><see cref="AutoMapper"/> instance</param>
        public UserFacade(Func<IUserRepository> userRepositoryFunc, Func<IUnitOfWorkProvider> uowFunc, IMapper mapper) : base(uowFunc)
        {
            this.userRepositoryFunc = userRepositoryFunc;
            this.mapper = mapper;
        }

        /// <summary>
        /// Adds new user into system and signing him in
        /// </summary>
        /// <param name="user">User to be added</param>
        /// <returns>Information about signed user</returns>
        /// <exception cref="BLException">Throw when email has been already used</exception>
        public async Task<UserSignedDTO> AddUserAsync(UserCreateDTO user)
        {
            using var uow = UowProviderFunc().Create();
            var repo = userRepositoryFunc();
            if (await repo.GetByEmailAsync(user?.Email) != null)
                throw new BLException(UserErrorCode.EmailAlreadyUsed, ErrorMessages.EmailAlreadyUsed);

            var entity = mapper.Map<User>(user);
            var (hash, salt) = SecurityHelper.CreateHash(user.Password);
            var currentDateTime = DateTime.Now;
            entity.PasswordHash = hash;
            entity.PasswordSalt = salt;
            entity.LastLoginOn = currentDateTime;
            entity.CreatedOn = currentDateTime;
            entity.TokenHash = Convert.ToBase64String(SecurityHelper.CreateHash(salt, user.Token));

            repo.Insert(entity);
            await uow.CommitAsync();

            var result = mapper.Map<UserSignedDTO>(entity);
            result.Token = user.Token;
            return result;
        }

        /// <summary>
        /// Gets user by identity
        /// </summary>
        /// <param name="id">User guid</param>
        /// <returns>User found by Id</returns>
        /// <exception cref="BLException">Throw when token provided does not match user token</exception>
        public async Task<UserDTO> VerifyAndGetUser(Guid id, string token)
        {
            using var uow = UowProviderFunc().Create();
            var repo = userRepositoryFunc();
            var user = await repo.GetByIdAsync(id, new StringPathIncludeDefinition<User>(nameof(User.Telephones)));
            if (user == null || !SecurityHelper.VerifyHashedPassword(user.TokenHash, user.PasswordSalt, token))
                throw new BLException(UserErrorCode.TokenMismatch, ErrorMessages.Unauthorized);

            return mapper.Map<UserDTO>(user);
        }

        /// <summary>
        /// Signs user in
        /// </summary>
        /// <param name="credentials">User credentials</param>
        /// <returns>Information about signed user</returns>
        /// <exception cref="BLException">Throw when user credentials are invalid</exception>
        public async Task<UserSignedDTO> SignInUser(UserCredentialsDTO credentials)
        {
            using var uow = UowProviderFunc().Create();
            var repo = userRepositoryFunc();
            var user = await repo.GetByEmailAsync(credentials.Email);
            if (user == null || !SecurityHelper.VerifyHashedPassword(user.PasswordHash, user.PasswordSalt, credentials.Password))
                throw new BLException(UserErrorCode.InvalidCredentials, ErrorMessages.InvalidCredentials);

            user.LastLoginOn = DateTime.Now;
            user.TokenHash = Convert.ToBase64String(SecurityHelper.CreateHash(user.PasswordSalt, credentials.Token));
            await uow.CommitAsync();

            var result = mapper.Map<UserSignedDTO>(user);
            result.Token = credentials.Token;
            return result;
        }
    }
}
