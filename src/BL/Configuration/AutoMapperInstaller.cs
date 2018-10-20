﻿using AutoMapper;
using BL.DTO;
using BL.DTO.User;
using DAL.Entities;

namespace BL.Configuration
{
    /// <summary>
    /// Static class for initializing Automapper
    /// </summary>
    public static class AutoMapperInstaller
    {
        private static bool isInitialized = false;
        private static readonly object locker = new object();

        /// <summary>
        /// Initializes automapper
        /// </summary>
        public static void Init()
        {
            if (isInitialized)
                return;

            lock (locker)
            {
                if (isInitialized)
                    return;

                isInitialized = true;
                Mapper.Initialize(config =>
                {
                    config.CreateMap<UserCreateDTO, User>();
                    config.CreateMap<User, UserDTO>();
                    config.CreateMap<User, UserSignedDTO>();

                    config.CreateMap<Phone, PhoneDTO>();
                    config.CreateMap<PhoneDTO, Phone>();
                });
            }
        }
    }
}