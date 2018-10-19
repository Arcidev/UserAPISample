using BL.Exceptions;
using System;

namespace RestAPI.Exceptions
{
    public class UnauthorizedException : Exception
    {
        public UnauthorizedException(string message) : base(message) { }

        public UnauthorizedException(BLException exception) : base(exception?.Message) { }
    }
}
