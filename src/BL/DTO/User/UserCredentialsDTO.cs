using System.ComponentModel.DataAnnotations;

namespace BL.DTO.User
{
    /// <summary>
    /// DTO for providing user credentials
    /// </summary>
    public class UserCredentialsDTO
    {
        /// <summary>
        /// User email
        /// </summary>
        [Required]
        public string Email { get; set; }

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
