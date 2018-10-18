using System;

namespace BL.DTO.User
{
    public class UserDTO : UserBaseDTO
    {
        public Guid Id { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? LastUpdatedOn { get; set; }

        public DateTime LastLoginOn { get; set; }
    }
}
