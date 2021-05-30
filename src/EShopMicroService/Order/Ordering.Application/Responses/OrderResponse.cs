using System;

namespace Ordering.Application.Responses
{
    public class OrderResponse
    {
        public Guid OrderId { get; set; }
        public Domain.Enums.OrderStatus OrderStatus { get; set; }
        public string ProductId { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime CreatedAt { get; set; }
        public string MerchantName { get; set; }
    }
}