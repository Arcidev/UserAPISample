using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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
        [Required, EmailAddress]
        public string Email { get; set; }

        /// <summary>
        /// User name
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// List of user phone numbers
        /// </summary>
        public List<PhoneDTO> Telephones { get; set; }
    }
}
