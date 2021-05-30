using System;
using Ordering.Domain.Entities.Base;
using Ordering.Domain.Enums;

namespace Ordering.Domain.Entities
{
    public class OrderShipment : EntityBase
    {
        public Guid OrderId { get; set; }
        public Guid CustomerId {get;set;}
        public ShipmentStatus ShipmentStatus { get; set; }
        public DateTime CreateAt { get; set; }
        public string Message { get; set; }

    }
}