using DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Riganti.Utils.Infrastructure.Core;
using System;
using System.Threading.Tasks;

namespace BL.Repositories
{
    public class UserRepository : BaseRepository<User, Guid>
    {
        public UserRepository(IUnitOfWorkProvider provider, IDateTimeProvider dateTimeProvider) : base(provider, dateTimeProvider) { }

        public async Task<User> GetByEmailAsync(string email)
        {
            return await Context.Users.FirstOrDefaultAsync(x => x.Email == email);
        }
    }
}
