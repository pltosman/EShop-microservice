using System;
using System.Net;
using System.Threading.Tasks;
using EShop.Core.Application.Commands.Abstractions;
using EShop.Core.Application.Commands.ResetPasswordCommands;
using EShop.Core.Application.Handlers;
using EShop.Core.Helpers;
using EShop.Core.Model.Enums;
using EShop.Core.Model.ResponseModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;


namespace EShop.Core.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class PasswordController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<PasswordController> _logger;

        public PasswordController(IMediator mediator,
            ILogger<PasswordController> logger)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _logger = logger;
        }


        [HttpPost("resetpassword")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(CommandResult), (int)HttpStatusCode.OK)]
        [ProducesDefaultResponseType(typeof(CommandResult))]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordCommand command)
        {
            var requestCommand = new IdentifiedCommand<ResetPasswordCommand, CommandResult>(command, Guid.NewGuid());

            _logger.LogInformation(
                "----- Sending command: {CommandName} - {IdProperty}: {CommandId} ({@Command})",
                requestCommand.GetGenericTypeName(),
                nameof(requestCommand.Command.Token),
                requestCommand.Command.Token,
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
