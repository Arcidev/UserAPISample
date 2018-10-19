using BL.Exceptions;
using System;

namespace RestAPI.Exceptions
{
    public class BadRequestException : Exception
    {
        public BadRequestException(BLException exception) : base(exception?.Message) { }
    }
}
