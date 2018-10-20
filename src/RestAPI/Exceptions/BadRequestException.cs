using BL.Exceptions;
using System;
using System.Net;

namespace RestAPI.Exceptions
{
    /// <summary>
    /// Exception for <see cref="HttpStatusCode.BadRequest"/>
    /// </summary>
    public class BadRequestException : Exception
    {
        /// <summary>
        /// Creates new instance of exception
        /// </summary>
        /// <param name="exception">Exception which caused result code 400</param>
        public BadRequestException(BLException exception) : base(exception?.Message) { }
    }
}
