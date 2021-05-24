using EShop.Core.Application.Commands.Abstractions;
using EShop.Core.Application.Commands.ConfirmationCommands;
using EShop.Core.Helpers;
using EShop.Core.Infrastructure.Data;
using EShop.Core.Infrastructure.Idempotency;
using EShop.Core.Model.Enums;
using EShop.Core.Model.ResponseModels;
using EShop.EventBus.Abstractions;
using EShop.Core.IntegrationEvents;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;


namespace EShop.Core.Application.Handlers
{
    public class EmailConfirmationCommandHandler : IRequestHandler<EmailConfirmationCommand, CommandResult>
    {
        private readonly IdentityContext _context;
        private readonly IEventBus _eventBus;

        public EmailConfirmationCommandHandler(IdentityContext context,
            IEventBus eventBus)
        {
            _context = context;
            _eventBus = eventBus;
        }

        public async Task<CommandResult> Handle(EmailConfirmationCommand command, CancellationToken cancellationToken)
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

            Random rnd = new();

            string otpCode = rnd.Next(100000, 999999).ToString();

            customerDetail.EmailConfirmationExpire = DateTime.Now.AddDays(1);
            customerDetail.EmailConfirmationToken = otpCode;
            _context.CustomerDetails.Update(customerDetail);

            await _context.SaveChangesAsync(cancellationToken);

            var customer = await _context.Customers.FindAsync(command.CustomerId);

            _eventBus.Publish(new EmailConfirmationEvent(customer.Email, otpCode));

            return CommandResult.GetSuccess(null, ResponseStatus.Success, StringResources.COMMAND_CUSTOMER_EMAIL_CONFIRMATIONCODESENT);
        }
    }

    public class EmailConfirmationIdentifiedCommandHandler : IdentifiedCommandHandler<EmailConfirmationCommand, CommandResult>
    {
        public EmailConfirmationIdentifiedCommandHandler(IMediator mediator,
            IRequestManager requestManager,
            ILogger<IdentifiedCommandHandler<EmailConfirmationCommand, CommandResult>> logger)
            : base(mediator, requestManager, logger)
        {
        }

        protected override CommandResult CreateRequestForDuplicatedRequest()
        {
            return CommandResult.GetError(ResponseStatus.Error, "Duplicated request !");
        }
    }
}
