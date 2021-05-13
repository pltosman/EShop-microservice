using System;

namespace Ordering.Application.Responses
{
    public class OrderResponse
    {
        public string ProductId { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime CreatedAt { get; set; }
        public string SellerUserName { get; set; }
    }
}