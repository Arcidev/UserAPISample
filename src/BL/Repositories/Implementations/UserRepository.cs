using DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Riganti.Utils.Infrastructure.Core;
using System;
using System.Threading.Tasks;

namespace BL.Repositories.Implementations
{
    /// <summary>
    /// Repository allowing operations on <see cref="User"/> entity
    /// </summary>
    public class UserRepository : BaseRepository<User, Guid>, IUserRepository
    {
        /// <summary>
        /// Creates new instance of User repository
        /// </summary>
        /// <param name="provider">Uow provider</param>
        /// <param name="dateTimeProvider">DateTime provider</param>
        public UserRepository(IUnitOfWorkProvider provider, IDateTimeProvider dateTimeProvider) : base(provider, dateTimeProvider) { }

        /// <inheritdoc />
        public async Task<User> GetByEmailAsync(string email)
        {
            return await Context.Users.FirstOrDefaultAsync(x => x.Email == email).ConfigureAwait(false);
        }
    }
}
