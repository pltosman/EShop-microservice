using System;
namespace EShop.Core.Helpers
{
    public class JsonWebToken
    {
        public Guid CustomerId { get; set; }
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public DateTime TokenExpire { get; set; }
    }
}
