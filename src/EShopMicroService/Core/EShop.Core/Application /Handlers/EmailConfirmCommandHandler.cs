using System;
using System.Threading;
using System.Threading.Tasks;
using EShop.Core.Application.Commands.ConfirmationCommands;
using EShop.Core.Infrastructure.Data;
using EShop.Core.Infrastructure.Idempotency;
using EShop.Core.Model.Enums;
using EShop.Core.Model.ResponseModels;
using MediatR;
using Microsoft.Extensions.Logging;
using EShop.Core.Helpers;

namespace EShop.Core.Application.Handlers
{
    public class EmailConfirmCommandHandler : IRequestHandler<EmailConfirmCommand, CommandResult>
    {
        private readonly IdentityContext _context;

        public EmailConfirmCommandHandler(IdentityContext context)
        {
            _context = context;
        }

        public async Task<CommandResult> Handle(EmailConfirmCommand command, CancellationToken cancellationToken)
        {
            var customerDetail = await _context.CustomerDetails.FindAsync(command.CustomerId);
            if (customerDetail == null)
            {
                return CommandResult.GetError(ResponseStatus.Error, StringResources.COMMAND_CUSTOMER_NOTFOUND);
            }

            if (customerDetail.EmailConfirmed)
            {
                return CommandResult.GetError(ResponseStatus.Error, StringResources.COMMAND_CUSTOMER_EMAIL_ALREADY_CONFIRMED);
            }

            if (customerDetail.EmailConfirmationExpire < DateTime.Now)
            {
                return CommandResult.GetError(ResponseStatus.Error, StringResources.COMMAND_CUSTOMER_EMAIL_TOKENEXPIRED);
            }

            if (customerDetail.EmailConfirmationToken == command.OtpCode)
            {
                customerDetail.EmailConfirmed = true;
                _context.CustomerDetails.Update(customerDetail);
                await _context.SaveChangesAsync(cancellationToken);

                return CommandResult.GetSuccess(null, ResponseStatus.Success, StringResources.COMMAND_CUSTOMER_EMAIL_CONFIRMATIONSUCCESS);
            }
            else
            {
                return CommandResult.GetError(ResponseStatus.Error, StringResources.COMMAND_CUSTOMER_EMAIL_INVALIDTOKEN);
            }
        }
    }

    public class EmailConfirmIdentifiedCommandHandler : IdentifiedCommandHandler<EmailConfirmCommand, CommandResult>
    {
        public EmailConfirmIdentifiedCommandHandler(IMediator mediator,
            IRequestManager requestManager,
            ILogger<IdentifiedCommandHandler<EmailConfirmCommand, CommandResult>> logger)
            : base(mediator, requestManager, logger)
        {
        }

        protected override CommandResult CreateRequestForDuplicatedRequest()
        {
            return CommandResult.GetError(ResponseStatus.Error, "Duplicated request !");
        }
    }
}
