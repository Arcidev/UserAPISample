using AutoMapper;
using BL.DTO.User;
using BL.Enums;
using BL.Exceptions;
using BL.Repositories;
using BL.Resources;
using DAL.Entities;
using Riganti.Utils.Infrastructure.Core;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace BL.Facades
{
    /// <summary>
    /// Facade for user oriented logic
    /// </summary>
    public class UserFacade : BaseFacade
    {
        private const int PBKDF2IterCount = 100000;
        private const int PBKDF2SubkeyLength = 160 / 8;
        private const int saltSize = 128 / 8;

        private readonly Func<IUserRepository> userRepositoryFunc;

        /// <summary>
        /// Creates new instance of User facade
        /// </summary>
        /// <param name="userRepositoryFunc">Functor of repository allowing operations on <see cref="User"/> entity</param>
        /// <param name="uowFunc">Functor for instantiating <see cref="IUnitOfWorkProvider"/></param>
        public UserFacade(Func<IUserRepository> userRepositoryFunc, Func<IUnitOfWorkProvider> uowFunc) : base(uowFunc)
        {
            this.userRepositoryFunc = userRepositoryFunc;
        }

        /// <summary>
        /// Adds new user into system and signing him in
        /// </summary>
        /// <param name="user">User to be added</param>
        /// <returns>Information about signed user</returns>
        /// <exception cref="BLException">Throw when email has been already used</exception>
        public async Task<UserSignedDTO> AddUserAsync(UserCreateDTO user)
        {
            using (var uow = UowProviderFunc().Create())
            {
                var repo = userRepositoryFunc();
                if (await repo.GetByEmailAsync(user?.Email) != null)
                    throw new BLException(UserErrorCode.EmailAlreadyUsed, ErrorMessages.EmailAlreadyUsed);

                var entity = Mapper.Map<User>(user);
                var (hash, salt) = CreateHash(user.Password);
                var currentDateTime = DateTime.Now;
                entity.PasswordHash = hash;
                entity.PasswordSalt = salt;
                entity.LastLoginOn = currentDateTime;
                entity.CreatedOn = currentDateTime;
                entity.TokenHash = Convert.ToBase64String(CreateHash(salt, user.Token));

                repo.Insert(entity);
                await uow.CommitAsync();

                return Mapper.Map<UserSignedDTO>(entity);
            }
        }

        /// <summary>
        /// Gets user by identity
        /// </summary>
        /// <param name="id">User guid</param>
        /// <returns>User found by Id</returns>
        public async Task<UserDTO> GetUserAsync(Guid id)
        {
            using (var uow = UowProviderFunc().Create())
            {
                var repo = userRepositoryFunc();
                var user = await repo.GetByIdAsync(id);

                return Mapper.Map<UserDTO>(user);
            }
        }

        /// <summary>
        /// Signs user in
        /// </summary>
        /// <param name="credentials">User credentials</param>
        /// <returns>Information about signed user</returns>
        /// <exception cref="BLException">Throw when user credentials are invalid</exception>
        public async Task<UserSignedDTO> SignInUser(UserCredentialsDTO credentials)
        {
            using (var uow = UowProviderFunc().Create())
            {
                var repo = userRepositoryFunc();
                var user = await repo.GetByEmailAsync(credentials.Email);
                if (user == null || !VerifyHashedPassword(user.PasswordHash, user.PasswordSalt, credentials.Password))
                    throw new BLException(UserErrorCode.InvalidCredentials, ErrorMessages.InvalidCredentials);

                user.LastLoginOn = DateTime.Now;
                user.TokenHash = Convert.ToBase64String(CreateHash(user.PasswordSalt, credentials.Token));
                await uow.CommitAsync();

                return Mapper.Map<UserSignedDTO>(user);
            }
        }

        private (string, string) CreateHash(string password)
        {
            using (var deriveBytes = new Rfc2898DeriveBytes(password, saltSize, PBKDF2IterCount))
            {
                var salt = deriveBytes.Salt;
                var subkey = deriveBytes.GetBytes(PBKDF2SubkeyLength);

                return (Convert.ToBase64String(subkey), Convert.ToBase64String(salt));
            }
        }

        private static byte[] CreateHash(string salt, string password)
        {
            var saltBytes = Convert.FromBase64String(salt);
            using (var deriveBytes = new Rfc2898DeriveBytes(password, saltBytes, PBKDF2IterCount))
            {
                return deriveBytes.GetBytes(PBKDF2SubkeyLength);
            }
        }

        private bool VerifyHashedPassword(string hashedPassword, string salt, string password)
        {
            var hashedPasswordBytes = Convert.FromBase64String(hashedPassword);
            return hashedPasswordBytes.SequenceEqual(CreateHash(salt, password));
        }
    }
}
