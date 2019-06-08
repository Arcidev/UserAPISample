using AutoMapper;
using BL.DTO;
using BL.DTO.User;
using DAL.Entities;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace BL.Configuration
{
    /// <summary>
    /// AutoMapper install helper
    /// </summary>
    public static class AutoMapperInstaller
    {
        /// <summary>
        /// Extends <see cref="IServiceCollection"/> to allow chained automapper installation
        /// </summary>
        /// <param name="services">DI container</param>
        /// <returns>Passed DI container to allow chaining</returns>
        public static IServiceCollection ConfigureAutoMapper(this IServiceCollection services)
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));

            var autoMapperConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<UserCreateDTO, User>();
                config.CreateMap<User, UserDTO>();
                config.CreateMap<User, UserSignedDTO>();

                config.CreateMap<Phone, PhoneDTO>();
                config.CreateMap<PhoneDTO, Phone>();
            });

            return services.AddSingleton<IConfigurationProvider>(autoMapperConfig)
                .AddSingleton<IMapper, Mapper>();
        }
    }
}
