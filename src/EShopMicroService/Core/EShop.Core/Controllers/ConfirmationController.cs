using System;
using System.Net;
using System.Threading.Tasks;
using EShop.Core.Application.Commands.Abstractions;
using EShop.Core.Application.Commands.ConfirmationCommands;
using EShop.Core.Application.Handlers;
using EShop.Core.Helpers;
using EShop.Core.Model.Enums;
using EShop.Core.Model.ResponseModels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using EShop.Core.Model.RequestModels;

namespace EShop.Core.Controllers
{
    [Authorize]
    [Route("api/account")]
    [ApiController]
    public class ConfirmationController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<ConfirmationController> _logger;

        public ConfirmationController(IMediator mediator,
            ILogger<ConfirmationController> logger)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _logger = logger;
        }

        [HttpPost("emailconfirmation")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(CommandResult), (int)HttpStatusCode.OK)]
        [ProducesDefaultResponseType(typeof(CommandResult))]
        public async Task<IActionResult> EmailConfirmation([FromBody] EmailConfirmationCommand command)
        {
           
            var requestCommand = new IdentifiedCommand<EmailConfirmationCommand, CommandResult>(command, Guid.NewGuid());

            _logger.LogInformation(
                "----- Sending command: {CommandName} - {IdProperty}: {CommandId} ({@Command})",
                requestCommand.GetGenericTypeName(),
                nameof(requestCommand.Command.CustomerId),
                requestCommand.Command.CustomerId,
                requestCommand);

            var commandResult = await _mediator.Send(command);

            if (commandResult == null)
            {
                return Ok(CommandResult.GetError(ResponseStatus.Error, StringResources.UNEXPECTED_ERROR));
            }

            return Ok(commandResult);
        }

        [HttpPost("emailconfirm")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(CommandResult), (int)HttpStatusCode.OK)]
        [ProducesDefaultResponseType(typeof(CommandResult))]
        public async Task<IActionResult> EmailConfirm([FromBody] EmailConfirmCommand command)
        {
          
            var requestCommand = new IdentifiedCommand<EmailConfirmCommand, CommandResult>(command, Guid.NewGuid());

            _logger.LogInformation(
                "----- Sending command: {CommandName} - {IdProperty}: {CommandId} ({@Command})",
                requestCommand.GetGenericTypeName(),
                nameof(requestCommand.Command.CustomerId),
                requestCommand.Command.CustomerId,
                requestCommand);

            var commandResult = await _mediator.Send(command);

            if (commandResult == null)
            {
                return Ok(CommandResult.GetError(ResponseStatus.Error, StringResources.UNEXPECTED_ERROR));
            }

            return Ok(commandResult);
        }
    }
}
