using System;
using Ordering.Domain.Enums;
namespace Ordering.Infrastructure.Exceptions
{
    public class OrderException : Exception
    {
        public ResponseStatus StatusCode => ResponseStatus.InternalServerError;
        public string ErrorCode => Guid.NewGuid().ToString().ToUpper();

        public OrderException()
        { }

        public OrderException(string message)
            : base(message)
        { }

        public OrderException(string message, Exception exception)
            : base(message, exception)
        { }
    }
}