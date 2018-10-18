using DAL.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DAL.Installers
{
    public static class DbInstaller
    {
        public static IServiceCollection ConfigureDatabase(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<AppDbContext>
                (options => options.UseSqlServer(connectionString));

            return services;
        }
    }
}
