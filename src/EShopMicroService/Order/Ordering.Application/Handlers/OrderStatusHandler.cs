using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Abstractions;
using Ordering.Application.Commands.OrderPaymentSuccess;
using Ordering.Application.Commands.OrderStatus;
using Ordering.Application.Responses;
using Ordering.Domain.Entities;
using Ordering.Domain.Enums;
using Ordering.Domain.Idempotency;
using Ordering.Domain.Repositories;

namespace Ordering.Application.Handlers
{
    public class OrderStatusHandler : IRequestHandler<OrderStatusCommand, CommandResult>
    {
        private readonly IOrderRepository _orderRepository;
        private IMapper _mapper;
        private readonly IMediator _mediator;
        public OrderStatusHandler(IMediator mediator, IOrderRepository orderRepository, IMapper mapper)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
            _mediator = mediator;
        }

        public async Task<CommandResult> Handle(OrderStatusCommand request, CancellationToken cancellationToken)
        {
            if (request.OrderId == Guid.Empty)
                throw new ApplicationException("OrderID could not be null");

            //TODO: You can create command and handler for all status change
            switch (request.OrderStatus)
            {
                case OrderStatus.OrderPaymentSuccess:
                    var paymentSuccessRequest = new IdentifiedCommand<OrderPaymentSuccessCommand, CommandResult>(new OrderPaymentSuccessCommand(request.OrderId, request.OrderStatus), request.OrderId);
                    await _mediator.Send(paymentSuccessRequest);
                    break;
                case OrderStatus.OrderShipped:
                    var shippedRequest = new IdentifiedCommand<OrderPaymentSuccessCommand, CommandResult>(new OrderPaymentSuccessCommand(request.OrderId, request.OrderStatus), request.OrderId);
                    await _mediator.Send(shippedRequest);
                    break;
                default:
                    break;
            }
            var orderEntity = await _orderRepository.GetByIdAsync(request.OrderId);

            var orderResponse = _mapper.Map<OrderResponse>(orderEntity);

            return CommandResult.GetSuccess(orderResponse,ResponseStatus.Success, "Order status changed."); ;
        }

    }

    public class OrderStatusIdentifiedCommandHandler : IdentifiedCommandHandler<OrderStatusCommand, CommandResult>
    {
        public OrderStatusIdentifiedCommandHandler(IMediator mediator,
            IRequestManager requestManager,
            ILogger<IdentifiedCommandHandler<OrderStatusCommand, CommandResult>> logger)
            : base(mediator, requestManager, logger)
        {
        }

        protected override CommandResult CreateRequestForDuplicatedRequest()
        {
            return CommandResult.GetError(ResponseStatus.Error, "Duplicated request !");
        }
    }
}