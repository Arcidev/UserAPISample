using DAL.Context;
using Riganti.Utils.Infrastructure.Core;
using Riganti.Utils.Infrastructure.EntityFrameworkCore;

namespace BL.Repositories.Implementations
{
    /// <summary>
    /// Base repository for shared logic
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    public abstract class BaseRepository<TEntity, TKey> : EntityFrameworkRepository<TEntity, TKey> where TEntity : class, IEntity<TKey>, new()
    {
        protected new AppDbContext Context => (AppDbContext)base.Context;

        protected BaseRepository(IUnitOfWorkProvider provider, IDateTimeProvider dateTimeProvider) : base(provider, dateTimeProvider) { }
    }
}
