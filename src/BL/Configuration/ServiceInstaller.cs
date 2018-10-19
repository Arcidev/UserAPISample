using BL.Repositories;
using BL.Repositories.Implementations;
using DAL.Installers;
using Microsoft.Extensions.DependencyInjection;
using Riganti.Utils.Infrastructure.Core;
using Riganti.Utils.Infrastructure.EntityFrameworkCore;
using System;

namespace BL.Configuration
{
    /// <summary>
    /// Service install helper
    /// </summary>
    public static class ServiceInstaller
    {
        /// <summary>
        /// Extends <see cref="IServiceCollection"/> to allow chained service installation
        /// </summary>
        /// <param name="services">DI container</param>
        /// <returns>Passed DI container to allow chaining</returns>
        public static IServiceCollection ConfigureServices(this IServiceCollection services, string connectionString)
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));

            return services.ConfigureDatabase(connectionString)
                .AddSingleton<IUnitOfWorkProvider, AppUnitOfWorkProvider>()
                .AddSingleton<IUnitOfWorkRegistry, AsyncLocalUnitOfWorkRegistry>()
                .AddSingleton<IDateTimeProvider, UtcDateTimeProvider>()
                .AddTransient<IUserRepository, UserRepository>()

                .AddTransient<Func<IUserRepository>>(provider => () => provider.GetService<IUserRepository>())
                .AddTransient<Func<IUnitOfWorkProvider>>(provider => () => provider.GetService<IUnitOfWorkProvider>())

                .AddTransient(typeof(IRepository<,>), typeof(EntityFrameworkRepository<,>));
        }
    }
}
