using System;
using EShop.Core.Helpers;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace EShop.Core.Application.Commands.ConfirmationCommands
{
    public class EmailConfirmCommandValidator : AbstractValidator<EmailConfirmCommand>
    {
        public EmailConfirmCommandValidator(ILogger<EmailConfirmCommandValidator> logger)
        {
            RuleFor(command => command.CustomerId)
                .NotEmpty().WithMessage(StringResources.COMMAND_CUSTOMERID_NOT_NULL);
 
            logger.LogTrace("----- INSTANCE CREATED - {ClassName}", GetType().Name);
        }
    }
}
