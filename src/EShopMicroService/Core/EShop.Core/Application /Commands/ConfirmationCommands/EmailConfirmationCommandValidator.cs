using EShop.Core.Helpers;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace EShop.Core.Application.Commands.ConfirmationCommands
{
    public class EmailConfirmationCommandValidator : AbstractValidator<EmailConfirmationCommand>
    {
        public EmailConfirmationCommandValidator(ILogger<EmailConfirmationCommandValidator> logger)
        {
            RuleFor(command => command.CustomerId)
                .NotEmpty().WithMessage(StringResources.COMMAND_CUSTOMERID_NOT_NULL);

            logger.LogTrace("----- INSTANCE CREATED - {ClassName}", GetType().Name);
        }
    }
}
