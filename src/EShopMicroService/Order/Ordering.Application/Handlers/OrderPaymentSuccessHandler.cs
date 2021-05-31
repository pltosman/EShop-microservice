using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using EShop.EventBus.Abstractions;
using EShop.EventBus.Extensions;
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
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEventBus _eventBus;
        private ILogger<OrderPaymentSuccessHandler> _logger;

        public OrderPaymentSuccessHandler(IUnitOfWork unitOfWork, IEventBus eventBus, ILogger<OrderPaymentSuccessHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _eventBus = eventBus;
            _logger = logger;
        }

        public async Task<CommandResult> Handle(OrderPaymentSuccessCommand request, CancellationToken cancellationToken)
        {
            var order = await _unitOfWork.Orders.GetByIdAsync(request.OrderId);
            if (order == null)
            {
                return CommandResult.GetError(ResponseStatus.Error, "Order could not find");
            }

            using (var transaction = _unitOfWork.BeginTransaction())
            {
                try
                {
                    //TODO: create shipment 
                    order.OrderStatus = request.OrderStatus;
                    await _unitOfWork.Orders.UpdateAsync(order);

                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();

                    _logger.LogError(
                                        "----- Request processing: {CommandName} - {IdProperty}: {CommandId} ({@Command})",
                request.GetGenericTypeName(),
                nameof(request.OrderId),
                request.OrderStatus,
                request);

                }
            }




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
