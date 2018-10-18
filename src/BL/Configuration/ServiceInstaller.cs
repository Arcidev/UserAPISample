using BL.Repositories;
using DAL.Context;
using DAL.Installers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Riganti.Utils.Infrastructure.Core;
using Riganti.Utils.Infrastructure.EntityFrameworkCore;
using System;

namespace BL.Configuration
{
    public static class ServiceInstaller
    {
        public static IServiceCollection ConfigureServices(this IServiceCollection services, string connectionString)
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));

            services.ConfigureDatabase(connectionString)
                .AddSingleton<IUnitOfWorkProvider, AppUnitOfWorkProvider>()
                .AddSingleton<IUnitOfWorkRegistry, AsyncLocalUnitOfWorkRegistry>()
                .AddSingleton<IDateTimeProvider, UtcDateTimeProvider>()
                .AddTransient<UserRepository, UserRepository>()

                .AddTransient<Func<DbContext>>(provider => () => provider.GetService<AppDbContext>())
                .AddTransient<Func<UserRepository>>(provider => () => provider.GetService<UserRepository>())
                .AddTransient<Func<IUnitOfWorkProvider>>(provider => () => provider.GetService<AppUnitOfWorkProvider>())

                .AddTransient(typeof(IRepository<,>), typeof(EntityFrameworkRepository<,>));

            return services;
        }
    }
}
