using System;
using Payment.Domain.Entities;

namespace Payment.Application.Responses
{
    public class PaymentResponse
    {
        public int OrderId { get; set; }

        public string CustomerId { get; set; }

        public Card Card { get; set; }

    }
}
