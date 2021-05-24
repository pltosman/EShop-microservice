using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EShop.Core.Application.Commands.RegisterCommands;
using EShop.Core.Domain.Entities;
using EShop.Core.Infrastructure.Data;
using EShop.Core.Infrastructure.Idempotency;
using EShop.Core.Model.Enums;
using EShop.Core.Model.ResponseModels;
using MediatR;
using Microsoft.Extensions.Logging;
using EShop.Core.Helpers;

namespace EShop.Core.Application.Handlers
{
    public class RegisterCustomerCommandHandler : IRequestHandler<RegisterCustomerCommand, CommandResult>
    {
        private readonly ILogger<RegisterCustomerCommandHandler> _logger;
        private readonly IdentityContext _context;

        public RegisterCustomerCommandHandler(ILogger<RegisterCustomerCommandHandler> logger, IdentityContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<CommandResult> Handle(RegisterCustomerCommand command, CancellationToken cancellationToken)
        {
           
            if (_context.Customers.Any(x => x.CustomerId == command.CustomerId))
            {
                return CommandResult.GetError(ResponseStatus.Error, StringResources.COMMAND_CUSTOMER_ALREADY_EXISTS);
            }

            if (_context.Customers.Any(x => x.Email == command.Email && x.Status != CustomerStatus.RegisteredButNotConfirmed))
            {
                return CommandResult.GetError(ResponseStatus.Error, StringResources.COMMAND_CUSTOMER_ALREADY_EXISTS);
            }

            EncryptionHelper.CreatePasswordHash(command.Password, out byte[] passwordHash, out byte[] passwordSalt);

            using var transaction = _context.Database.BeginTransaction();

            try
            {
                var customer = new Customer
                {
                    CustomerId = command.CustomerId,
                    PasswordHash = passwordHash,
                    PasswordSalt = passwordSalt,
                    FirstName = StringHelper.GetFirstName(command.FirstName),
                    LastName = StringHelper.GetFirstName(command.LastName),
                    Email = command.Email,
                    FullName = StringHelper.GetFullName(command.FirstName, command.LastName),
                    Status = CustomerStatus.RegisteredButNotConfirmed
                };

                _context.Customers.Add(customer);

                _context.CustomerDetails.Add(new CustomerDetail
                {
                    CustomerId = command.CustomerId,
                    RegistrationOn = DateTime.Now,
                    EmailConfirmed = false,
                 
                });

                _ = await _context.SaveChangesAsync(cancellationToken);

                transaction.Commit();

                return CommandResult.GetSuccess(customer, ResponseStatus.Success);
            }
            catch (Exception ex)
            {
                _logger.LogError("RegisterCustomerCommandHandler Transaction Error: {@ex}", ex.InnerException != null ? ex.InnerException.Message : ex.Message);

                await transaction.RollbackAsync(cancellationToken);
            }

            return null;
        }
    }

    public class RegisterCustomerIdentifiedCommandHandler : IdentifiedCommandHandler<RegisterCustomerCommand, CommandResult>
    {
        public RegisterCustomerIdentifiedCommandHandler(IMediator mediator,
            IRequestManager requestManager,
            ILogger<IdentifiedCommandHandler<RegisterCustomerCommand, CommandResult>> logger)
            : base(mediator, requestManager, logger)
        {
        }

        protected override CommandResult CreateRequestForDuplicatedRequest()
        {
            return CommandResult.GetError(ResponseStatus.Error, "Duplicated request !");
        }
    }
}
