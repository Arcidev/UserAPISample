using Microsoft.EntityFrameworkCore;
using Riganti.Utils.Infrastructure.Core;
using Riganti.Utils.Infrastructure.EntityFrameworkCore;
using System;
using DbContextOptions = Riganti.Utils.Infrastructure.EntityFrameworkCore.DbContextOptions;

namespace BL
{
    public class AppUnitOfWorkProvider : EntityFrameworkUnitOfWorkProvider
    {
        public AppUnitOfWorkProvider(IUnitOfWorkRegistry registry, Func<DbContext> dbContextFactory) : base(registry, dbContextFactory) { }

        protected override EntityFrameworkUnitOfWork<DbContext> CreateUnitOfWork(Func<DbContext> dbContextFactory, DbContextOptions options)
        {
            return new AppUnitOfWork(this, dbContextFactory, options);
        }
    }
}
