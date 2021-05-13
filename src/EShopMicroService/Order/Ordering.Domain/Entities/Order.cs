using System;
using Ordering.Domain.Entities.Base;

namespace Ordering.Domain.Entities
{
    public class Order : EntityBase
    {
        public string ProductId { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime CreatedAt { get; set; }
        public string SellerUserName { get; set; }
    }
}