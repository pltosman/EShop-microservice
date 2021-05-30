using System;
using Ordering.Domain.Enums;

namespace Ordering.Application.Responses
{
    public class CommandResult
    {
        public ResponseStatus Status { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }
        public string ErrorCode { get; set; }

        public static CommandResult GetError(ResponseStatus status, string message)
        {
            return new CommandResult
            {
                ErrorCode = Guid.NewGuid().ToString().ToUpper(),
                Data = default,
                Message = message,
                Status = status
            };
        }

        public static CommandResult GetError(object data, ResponseStatus status, string message)
        {
            return new CommandResult
            {
                ErrorCode = Guid.NewGuid().ToString().ToUpper(),
                Data = data,
                Message = message,
                Status = status
            };
        }

        public static CommandResult GetSuccess(object data, ResponseStatus status, string message = null)
        {
            return new CommandResult
            {
                Data = data,
                Message = message,
                Status = status
            };
        }
    }

    public static class CommandResultHelper
    {
        public static T GetT<T>(this CommandResult commandResult)
        {
            return (T)commandResult.Data;
        }
    }
}
