using System;
using System.Threading.Tasks;
using EShop.EventBus.Abstractions;
using IntegrationEvents.Events;
using MediatR;
using Ordering.Application.Abstractions;
using Ordering.Application.Commands.OrderStatus;
using Ordering.Application.Responses;

namespace EShop.Order.IntegrationEvents.EventHandling
{
    public class OrderStatusEventHandler : IIntegrationEventHandler<OrderStatusEvent>
    {
        private readonly IMediator _mediator;

        public OrderStatusEventHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task Handle(OrderStatusEvent @event)
        {
            var result = await _mediator.Send(new IdentifiedCommand<OrderStatusCommand, CommandResult>(new OrderStatusCommand(@event.OrderId, DateTime.UtcNow, @event.OrderStatus), Guid.NewGuid()));

            throw new NotImplementedException();
        }
    }
}
