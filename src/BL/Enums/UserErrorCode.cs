
namespace BL.Enums
{
    /// <summary>
    /// User error codes
    /// </summary>
    public enum UserErrorCode
    {
        /// <summary>
        /// Email has already been used
        /// </summary>
        EmailAlreadyUsed = 1,

        /// <summary>
        /// Provided credentials were invalid
        /// </summary>
        InvalidCredentials,

        /// <summary>
        /// Provided email is not valid
        /// </summary>
        InvalidEmail,

        /// <summary>
        /// Provided token does not match user token
        /// </summary>
        TokenMismatch
    }
}
