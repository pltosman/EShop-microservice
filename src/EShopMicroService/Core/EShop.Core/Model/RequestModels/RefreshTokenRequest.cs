using System;
namespace EShop.Core.Model.RequestModels
{
    public class RefreshTokenRequest
    {
        public Guid CustomerId { get; set; }
        public string RefreshToken { get; set; }
    }
}

