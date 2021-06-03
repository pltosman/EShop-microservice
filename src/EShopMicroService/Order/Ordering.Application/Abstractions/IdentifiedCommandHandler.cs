using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Commands.OrderDelivered;
using Ordering.Application.Commands.OrderPaymentReject;
using Ordering.Application.Commands.OrderPaymentSuccess;
using Ordering.Application.Commands.OrderShipped;
using Ordering.Application.Commands.OrderStatus;
using Ordering.Domain.Helpers;
using Ordering.Domain.Idempotency;

namespace Ordering.Application.Abstractions
{
    public class IdentifiedCommandHandler<T, R> : IRequestHandler<IdentifiedCommand<T, R>, R>
          where T : IRequest<R>
    {
        private readonly IMediator _mediator;
        private readonly IRequestManager _requestManager;
        private readonly ILogger<IdentifiedCommandHandler<T, R>> _logger;

        public IdentifiedCommandHandler(IMediator mediator, IRequestManager requestManager, ILogger<IdentifiedCommandHandler<T, R>> logger)
        {
            _mediator = mediator;
            _requestManager = requestManager;
            _logger = logger;
        }

        public async Task<R> Handle(IdentifiedCommand<T, R> message, CancellationToken cancellationToken)
        {
            var alreadyExists = await _requestManager.ExistAsync(message.Id);
            if (alreadyExists)
            {
                return CreateRequestForDuplicatedRequest();
            }
            else
            {
                await _requestManager.CreateRequestForCommandAsync<T>(message.Id);
                try
                {
                    var command = message.Command;
                    var commandName = command.GetGenericTypeName();
                    var idProperty = string.Empty;
                    var commandId = string.Empty;


                    switch (command)
                    {

                        case OrderStatusCommand orderStatusCommand:
                            idProperty = nameof(orderStatusCommand.OrderId);
                            commandId = orderStatusCommand.OrderStatus.ToString();
                            break;

                        case OrderPaymentSuccessCommand orderPaymentSuccessfulCommand:
                            idProperty = nameof(orderPaymentSuccessfulCommand.OrderId);
                            commandId = orderPaymentSuccessfulCommand.OrderStatus.ToString();
                            break;

                        case OrderPaymentRejectCommand orderPaymentRejectCommand:
                            idProperty = nameof(orderPaymentRejectCommand.OrderId);
                            commandId = orderPaymentRejectCommand.OrderStatus.ToString();
                            break;

                        case OrderShippedCommand orderShippedCommand:
                            idProperty = nameof(orderShippedCommand.OrderId);
                            commandId = orderShippedCommand.OrderStatus.ToString();
                            break;

                        case OrderDeliveredCommand orderDeliveredCommand:
                            idProperty = nameof(orderDeliveredCommand.OrderId);
                            commandId = orderDeliveredCommand.OrderStatus.ToString();
                            break;

                        default:
                            idProperty = "Id?";
                            commandId = "n/a";
                            break;
                    }

                    _logger.LogInformation(
                        "----- Sending command: {CommandName} - {IdProperty}: {CommandId} ({@Command})",
                        commandName,
                        idProperty,
                        commandId,
                        command);

                    var result = await _mediator.Send(command, cancellationToken);

                    _logger.LogInformation(
                        "----- Command result: {@Result} - {CommandName} - {IdProperty}: {CommandId} ({@Command})",
                        result,
                        commandName,
                        idProperty,
                        commandId,
                        command);

                    return result;
                }
                catch
                {
                    return default;
                }
            }

        }

        protected virtual R CreateRequestForDuplicatedRequest()
        {
            return default;
        }
    }
}
