using System;
using EShop.Core.Model.Enums;

namespace EShop.Core.Infrastructure.Exceptions
{
    public class IdentityException : Exception
    {
        public ResponseStatus StatusCode => ResponseStatus.InternalServerError;
        public string ErrorCode => Guid.NewGuid().ToString().ToUpper();

        public IdentityException()
        { }

        public IdentityException(string message)
            : base(message)
        { }

        public IdentityException(string message, Exception exception)
            : base(message, exception)
        { }
    }
}

