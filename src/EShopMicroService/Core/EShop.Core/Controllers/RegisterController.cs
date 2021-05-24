using System;
using System.Net;
using System.Threading.Tasks;
using EShop.Core.Application.Commands.Abstractions;
using EShop.Core.Application.Commands.RegisterCommands;
using EShop.Core.Application.Handlers;
using EShop.Core.Domain.Entities;
using EShop.Core.Helpers;
using EShop.Core.Model.Enums;
using EShop.Core.Model.ResponseModels;
using EShop.Core.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;


namespace EShop.Core.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class RegisterController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<RegisterController> _logger;
        private readonly IAccountService _accountService;

        public RegisterController(IMediator mediator,
            ILogger<RegisterController> logger,
            IAccountService accountService)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _logger = logger;
            _accountService = accountService;
        }

        [HttpPost("register")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(JsonWebToken), (int)HttpStatusCode.OK)]
        [ProducesDefaultResponseType(typeof(CommandResult))]
        public async Task<IActionResult> RegisterCustomer([FromBody] RegisterCustomerCommand command)
        {  

            var requestCommand = new IdentifiedCommand<RegisterCustomerCommand, CommandResult>(command, Guid.NewGuid());

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

            if (commandResult?.Status == ResponseStatus.Success)
            {
                Guid customerId = commandResult.GetT<Customer>().CustomerId;

                var token = _accountService.CrateToken(customerId);

                commandResult = CommandResult.GetSuccess(token, ResponseStatus.Success);
            }

            return Ok(commandResult);
        }

    }
}