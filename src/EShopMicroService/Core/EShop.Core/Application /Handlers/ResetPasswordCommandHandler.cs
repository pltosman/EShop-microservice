using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EShop.Core.Application.Commands.Abstractions;
using EShop.Core.Application.Commands.ResetPasswordCommands;
using EShop.Core.Helpers;
using EShop.Core.Infrastructure.Data;
using EShop.Core.Infrastructure.Idempotency;
using EShop.Core.Model.Enums;
using EShop.Core.Model.ResponseModels;
using MediatR;
using Microsoft.Extensions.Logging;

namespace EShop.Core.Application.Handlers
{
    public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand, CommandResult>
    {
        private readonly IdentityContext _context;

        public ResetPasswordCommandHandler(IdentityContext context)
        {
            _context = context;
        }

        public async Task<CommandResult> Handle(ResetPasswordCommand command, CancellationToken cancellationToken)
        {
            var customerDetail = _context.CustomerDetails.FirstOrDefault(x => x.PasswordReminderToken == command.Token);
            if (customerDetail is null)
            {
                return CommandResult.GetError(ResponseStatus.Error, StringResources.COMMAND_CUSTOMER_NOTFOUND);
            }

            if (customerDetail.PasswordReminderExpire < DateTime.Now)
            {
                return CommandResult.GetError(ResponseStatus.Error, StringResources.COMMAND_CUSTOMER_RESETPASSWORD_TOKENEXPIRED);
            }

            var customer = _context.Customers.FirstOrDefault(x => x.CustomerId == customerDetail.CustomerId);
            if (customer is null)
            {
                return CommandResult.GetError(ResponseStatus.Error, StringResources.COMMAND_CUSTOMER_NOTFOUND);
            }

            if (customerDetail.PasswordReminderToken == command.Token)
            {
                EncryptionHelper.CreatePasswordHash(command.NewPassword, out byte[] passwordHash, out byte[] passwordSalt);

                customerDetail.PasswordReminderToken = null;
                customerDetail.PasswordReminderExpire = null;
                _context.CustomerDetails.Update(customerDetail);

                customer.PasswordHash = passwordHash;
                customer.PasswordSalt = passwordSalt;
                _context.Customers.Update(customer);

                await _context.SaveChangesAsync(cancellationToken);

                return CommandResult.GetSuccess(null, ResponseStatus.Success, StringResources.COMMAND_CUSTOMER_RESETPASSWORD_SUCCESS);
            }
            else
            {
                return CommandResult.GetError(ResponseStatus.Error, StringResources.COMMAND_CUSTOMER_EMAIL_INVALIDTOKEN);
            }
        }
    }

    public class ResetPasswordIdentifiedCommandHandler : IdentifiedCommandHandler<ResetPasswordCommand, CommandResult>
    {
        public ResetPasswordIdentifiedCommandHandler(IMediator mediator,
            IRequestManager requestManager,
            ILogger<IdentifiedCommandHandler<ResetPasswordCommand, CommandResult>> logger)
            : base(mediator, requestManager, logger)
        {
        }

        protected override CommandResult CreateRequestForDuplicatedRequest()
        {
            return CommandResult.GetError(ResponseStatus.Error, "Duplicated request !");
        }
    }
}
