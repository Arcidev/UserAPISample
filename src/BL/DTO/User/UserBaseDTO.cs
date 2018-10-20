using System.Collections.Generic;

namespace BL.DTO.User
{
    /// <summary>
    /// DTO containing basic information about user
    /// </summary>
    public class UserBaseDTO
    {
        /// <summary>
        /// User email
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// User name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// List of user phone numbers
        /// </summary>
        public List<PhoneDTO> Telephones { get; set; }
    }
}
