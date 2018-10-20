using BL.Exceptions;
using System;

namespace RestAPI.Exceptions
{
    public class UnauthorizedException : Exception
    {
        public UnauthorizedException(string message, Exception innerException = null) : base(message, innerException) { }

        public UnauthorizedException(BLException exception) : base(exception?.Message) { }
    }
}
