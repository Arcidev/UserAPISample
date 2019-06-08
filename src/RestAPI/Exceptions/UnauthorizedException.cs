using BL.Exceptions;
using System;
using System.Net;

namespace RestAPI.Exceptions
{
    /// <summary>
    /// Exception for <see cref="HttpStatusCode.Unauthorized"/>
    /// </summary>
    public class UnauthorizedException : Exception
    {
        /// <summary>
        /// Creates new instance of exception
        /// </summary>
        /// <param name="message">Description</param>
        /// <param name="innerException">Exception which caused result code 401</param>
        public UnauthorizedException(string message, Exception innerException) : base(message, innerException) { }

        /// <summary>
        /// Creates new instance of exception
        /// </summary>
        /// <param name="exception">Exception which caused result code 401</param>
        public UnauthorizedException(BLException exception) : base(exception?.Message) { }
    }
}
