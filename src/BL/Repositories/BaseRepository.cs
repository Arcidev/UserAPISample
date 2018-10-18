using DAL.Context;
using Riganti.Utils.Infrastructure.Core;
using Riganti.Utils.Infrastructure.EntityFrameworkCore;

namespace BL.Repositories
{
    public class BaseRepository<TEntity, TKey> : EntityFrameworkRepository<TEntity, TKey> where TEntity : class, IEntity<TKey>, new()
    {
        protected new AppDbContext Context => (AppDbContext)base.Context;

        public BaseRepository(IUnitOfWorkProvider provider, IDateTimeProvider dateTimeProvider) : base(provider, dateTimeProvider) { }
    }
}
