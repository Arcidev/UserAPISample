using AutoMapper;
using BL.DTO;
using BL.DTO.User;
using DAL.Entities;

namespace BL.Configuration
{
    public static class AutoMapper
    {
        public static void Init()
        {
            Mapper.Initialize(config =>
            {
                config.CreateMap<UserCreateDTO, User>();
                config.CreateMap<User, UserDTO>();
                config.CreateMap<User, UserSignedDTO>();

                config.CreateMap<Phone, PhoneDTO>();
            });
        }
    }
}
