using System;
using EShop.Core.Infrastructure.Idempotency;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;
using EShop.Core.Application.Commands.Abstractions;
using EShop.Core.Helpers;
using EShop.Core.Application.Commands.ConfirmationCommands;
using EShop.Core.Application.Commands.RegisterCommands;
using EShop.Core.Application.Commands.ResetPasswordCommands;

namespace EShop.Core.Application.Handlers
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
                        case EmailConfirmationCommand emailConfirmationCommand:
                            idProperty = nameof(emailConfirmationCommand.CustomerId);
                            commandId = emailConfirmationCommand.CustomerId.ToString();
                            break;

                        case EmailConfirmCommand emailConfirmCommand:
                            idProperty = nameof(emailConfirmCommand.CustomerId);
                            commandId = emailConfirmCommand.CustomerId.ToString();
                            break;

                        case RegisterCustomerCommand registerCustomerCommand:
                            idProperty = nameof(registerCustomerCommand.CustomerId);
                            commandId = registerCustomerCommand.CustomerId.ToString();
                            break;

                        case ResetPasswordCommand resetPasswordCommand:
                            idProperty = nameof(resetPasswordCommand.Token);
                            commandId = resetPasswordCommand.Token;
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
