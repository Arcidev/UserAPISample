using AutoMapper;
using BL.DTO.User;
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
    public class UserFacade : BaseFacade
    {
        private const int PBKDF2IterCount = 100000;
        private const int PBKDF2SubkeyLength = 160 / 8;
        private const int saltSize = 128 / 8;

        private readonly Func<UserRepository> userRepositoryFunc;

        public UserFacade(Func<UserRepository> userRepository, Func<IUnitOfWorkProvider> uowFunc) : base(uowFunc)
        {
            userRepositoryFunc = userRepository;
        }

        public async Task<UserSignedDTO> AddUserAsync(UserCreateDTO user)
        {
            using (var uow = UowProviderFunc().Create())
            {
                var repo = userRepositoryFunc();
                if (GetUserByEmailAsync(user?.Email) != null)
                    throw new UIException(ErrorMessages.EmailAlreadyUsed);

                var entity = Mapper.Map<User>(user);
                var (hash, salt) = CreateHash(user.Password);
                var currentDateTime = DateTime.Now;
                entity.PasswordHash = hash;
                entity.PasswordSalt = salt;
                entity.LastLoginOn = currentDateTime;
                entity.CreatedOn = currentDateTime;

                repo.Insert(entity);
                await uow.CommitAsync();

                return Mapper.Map<UserSignedDTO>(entity);
            }
        }

        public async Task<UserDTO> GetUserByEmailAsync(string email)
        {
            using (var uow = UowProviderFunc().Create())
            {
                var repo = userRepositoryFunc();
                var user = await repo.GetByEmailAsync(email);

                return Mapper.Map<UserDTO>(user);
            }
        }

        public async Task<UserSignedDTO> SignInUser(UserCredentialsDTO credentials)
        {
            using (var uow = UowProviderFunc().Create())
            {
                var repo = userRepositoryFunc();
                var user = await repo.GetByEmailAsync(credentials.Email);
                if (user == null || !VerifyHashedPassword(user.PasswordHash, user.PasswordSalt, credentials.Password))
                    throw new UIException(ErrorMessages.InvalidCredentials);

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

        private bool VerifyHashedPassword(string hashedPassword, string salt, string password)
        {
            byte[] hashedPasswordBytes = Convert.FromBase64String(hashedPassword);
            byte[] saltBytes = Convert.FromBase64String(salt);

            using (var deriveBytes = new Rfc2898DeriveBytes(password, saltBytes, PBKDF2IterCount))
            {
                byte[] generatedSubkey = deriveBytes.GetBytes(PBKDF2SubkeyLength);
                return hashedPasswordBytes.SequenceEqual(generatedSubkey);
            }
        }
    }
}
