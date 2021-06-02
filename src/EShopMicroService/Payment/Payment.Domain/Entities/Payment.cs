using System;
using Payment.Domain.Entities.Base;

namespace Payment.Domain.Entities
{
    public class Payment : EntityBase
    {
        public Guid  OrderId { get; set; }

        public string CustomerId { get; set; }

        public Card Card { get; set; }

    }
}
