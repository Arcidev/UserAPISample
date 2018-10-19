using BL.Facades;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace BL.Configuration
{
    /// <summary>
    /// Facade install helper
    /// </summary>
    public static class FacadeInstaller
    {
        /// <summary>
        /// Extends <see cref="IServiceCollection"/> to allow chained facade installation
        /// </summary>
        /// <param name="services">DI container</param>
        /// <returns>Passed DI container to allow chaining</returns>
        public static IServiceCollection ConfigureFacades(this IServiceCollection services)
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));

            return services.AddTransient<UserFacade, UserFacade>();
        }
    }
}
