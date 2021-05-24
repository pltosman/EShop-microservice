using System;
using System.ComponentModel.DataAnnotations;
using EShop.Core.Domain.Entities.Base;

namespace EShop.Core.Domain.Entities
{
    public class CustomerToken : IEntityBase
    {
        [Key]
        public Guid CustomerTokenId { get; set; }
        public Guid CustomerId { get; set; }
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public DateTime TokenExpire { get; set; }
    }
}

