using System;
using EShop.EventBus.Events;
using Ordering.Domain.Enums;

namespace IntegrationEvents.Events
{
    public class OrderStatusEvent : IntegrationEvent
    {
        public Guid OrderId { get; set; }
        public OrderStatus OrderStatus { get; set; }


        public OrderStatusEvent(Guid orderId, OrderStatus orderStatus)
        {
            OrderId = orderId;
            OrderStatus = orderStatus;
        }
    }
}
