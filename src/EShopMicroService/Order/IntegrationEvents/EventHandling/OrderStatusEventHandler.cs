using System;
using System.Threading.Tasks;
using EShop.EventBus.Abstractions;
using IntegrationEvents.Events;

namespace IntegrationEvents.EventHandling
{
    public class OrderStatusEventHandler : IIntegrationEventHandler<OrderStatusEvent>
    {
       

        public Task Handle(OrderStatusEvent @event)
        {
           // Call order status change mothod
        }
    }
}
