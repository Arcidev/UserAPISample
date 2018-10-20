using System;

namespace BL.DTO.User
{
    /// <summary>
    /// DTO for signed user response
    /// </summary>
    public class UserSignedDTO
    {
        /// <summary>
        /// User Id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Date and time when user was created
        /// </summary>
        public DateTime CreatedOn { get; set; }

        /// <summary>
        /// Date and time when user was last updated
        /// </summary>
        public DateTime? LastUpdatedOn { get; set; }

        /// <summary>
        /// Date and time when user was last logged into system
        /// </summary>
        public DateTime LastLoginOn { get; set; }

        /// <summary>
        /// User token
        /// </summary>
        public string Token { get; set; }
    }
}
