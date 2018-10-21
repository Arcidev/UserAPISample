
using System.ComponentModel.DataAnnotations;

namespace BL.DTO.User
{
    /// <summary>
    /// DTO used for creating user
    /// </summary>
    public class UserCreateDTO : UserBaseDTO
    {
        /// <summary>
        /// User password
        /// </summary>
        [Required]
        public string Password { get; set; }

        /// <summary>
        /// User token
        /// </summary>
        public string Token { get; set; }
    }
}
