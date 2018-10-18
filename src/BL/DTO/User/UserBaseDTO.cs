using System;
using System.Collections.Generic;

namespace BL.DTO.User
{
    public class UserBaseDTO
    {
        public string Email { get; set; }

        public string Name { get; set; }

        public List<PhoneDTO> Telephones { get; set; }
    }
}
