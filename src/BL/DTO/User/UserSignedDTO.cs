using System;

namespace BL.DTO.User
{
    public class UserSignedDTO
    {
        public Guid Id { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? LastUpdatedOn { get; set; }

        public DateTime LastLoginOn { get; set; }

        public string Token { get; set; }
    }
}
