using System;
using Ordering.Domain.Entities.Base;
using Ordering.Domain.Enums;

namespace Ordering.Domain.Entities
{
    public class Order : EntityBase
    {
        public string ProductId { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime CreatedAt { get; set; }
        public string MerchantName { get; set; }
        public OrderStatus OrderStatus { get; set; } = OrderStatus.WaitingForPayment;
    }
}