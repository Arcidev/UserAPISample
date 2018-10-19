using BL.Enums;
using System;

namespace BL.Exceptions
{
    /// <summary>
    /// Logical exception used by this layer
    /// </summary>
    public class BLException : Exception
    {
        /// <summary>
        /// User error code
        /// </summary>
        public UserErrorCode UserErrorCode { get; }

        /// <summary>
        /// Creates new instance of exception
        /// </summary>
        /// <param name="errorCode">Error code which raised this exception</param>
        /// <param name="message">User friendly message</param>
        public BLException(UserErrorCode errorCode, string message) : base(message)
        {
            UserErrorCode = errorCode;
        }
    }
}
