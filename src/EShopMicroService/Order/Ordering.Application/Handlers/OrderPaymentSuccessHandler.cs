using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using EShop.EventBus.Abstractions;
using IntegrationEvents.Events;
using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Abstractions;
using Ordering.Application.Commands.OrderPaymentSuccess;
using Ordering.Application.Responses;
using Ordering.Domain.Enums;
using Ordering.Domain.Idempotency;
using Ordering.Domain.Repositories;

namespace Ordering.Application.Handlers
{
    public class OrderPaymentSuccessHandler : IRequestHandler<OrderPaymentSuccessCommand, CommandResult>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IEventBus _eventBus;

        public OrderPaymentSuccessHandler(IOrderRepository orderRepository, IEventBus eventBus)
        {
            _orderRepository = orderRepository;
            _eventBus = eventBus;
        }

        public async Task<CommandResult> Handle(OrderPaymentSuccessCommand request, CancellationToken cancellationToken)
        {
            var order = await _orderRepository.GetByIdAsync(request.OrderId);
            if (order == null)
            {
                return CommandResult.GetError(ResponseStatus.Error, "Order could not find");
            }



            //TODO: UoW yapısı kurup bu burada begin end tranc yapısı olmalı dır. 


            order.OrderStatus = request.OrderStatus;
            await _orderRepository.UpdateAsync(order);

            // If the payment is for a book, create a duplicate packing slip for the royalty department.
            // If the payment is for a physical product or a book, generate a commission payment to the agent.
            _eventBus.Publish(new ShipmentEvent(order.MerchantName, order.Id, order.TotalPrice));

            return CommandResult.GetSuccess(null, ResponseStatus.Success, "Order status has changed successfully.");
        }
    }

    public class OrderPaymentSuccessCommandHandler : IdentifiedCommandHandler<OrderPaymentSuccessCommand, CommandResult>
    {
        public OrderPaymentSuccessCommandHandler(IMediator mediator,
            IRequestManager requestManager,
            ILogger<IdentifiedCommandHandler<OrderPaymentSuccessCommand, CommandResult>> logger)
            : base(mediator, requestManager, logger)
        {
        }

        protected override CommandResult CreateRequestForDuplicatedRequest()
        {
            return CommandResult.GetError(ResponseStatus.Error, "Duplicated request !");
        }
    }
}
