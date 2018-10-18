using BL.Facades;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace BL.Configuration
{
    public static class FacadeInstaller
    {
        public static IServiceCollection ConfigureFacades(this IServiceCollection services)
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));

            services.AddTransient<UserFacade, UserFacade>();

            return services;
        }
    }
}
