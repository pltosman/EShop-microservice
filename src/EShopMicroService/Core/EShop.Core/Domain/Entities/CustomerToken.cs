using System;
using EShop.Core.Domain.Entities.Base;

namespace EShop.Core.Domain.Entities
{
    public class CustomerToken : EntityBase
    {
     
        public Guid CustomerId { get; set; }
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public DateTime TokenExpire { get; set; }
    }
}

