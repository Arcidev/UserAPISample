using DAL.Context;
using Microsoft.EntityFrameworkCore;
using Riganti.Utils.Infrastructure.Core;
using Riganti.Utils.Infrastructure.EntityFrameworkCore;
using System;
using DbContextOptions = Riganti.Utils.Infrastructure.EntityFrameworkCore.DbContextOptions;

namespace BL
{
    public class AppUnitOfWork : EntityFrameworkUnitOfWork
    {
        public new AppDbContext Context => (AppDbContext)base.Context;

        public AppUnitOfWork(IUnitOfWorkProvider provider, Func<DbContext> dbContextFactory, DbContextOptions options) : base(provider, dbContextFactory, options) { }
    }
}
