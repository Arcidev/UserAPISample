using DAL.Entities;
using Riganti.Utils.Infrastructure.Core;
using System;
using System.Threading.Tasks;

namespace BL.Repositories
{
    public interface IUserRepository
    {
        /// <summary>
        /// Gets user by email
        /// </summary>
        /// <param name="email">Email to filter user by</param>
        /// <returns>User entity</returns>
        Task<User> GetByEmailAsync(string email);

        /// <summary>
        /// Gets user by id
        /// </summary>
        /// <param name="id">Id to filter user by</param>
        /// <param name="includes">Entity includes</param>
        /// <returns>User entity</returns>
        Task<User> GetByIdAsync(Guid id, params IIncludeDefinition<User>[] includes);

        /// <summary>
        /// Inserts user to repository
        /// </summary>
        /// <param name="entity">Entity to be inserted</param>
        void Insert(User entity);
    }
}
