namespace Ordering.Domain.Enums
{
   public enum ResponseStatus
    {
        Success = 100,
        Error = 101,
        InternalServerError = 102,
        ValidationError = 103,
        Unauthorized = 201,
        AnotherDeviceLogin = 202
    }
}