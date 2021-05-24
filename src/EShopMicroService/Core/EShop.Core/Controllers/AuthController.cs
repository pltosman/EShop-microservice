using System;
using System.Net;
using EShop.Core.Model.ResponseModels;
using EShop.Core.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using EShop.Core.Model.RequestModels;
using EShop.Core.Helpers;

namespace EShop.Core.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ILogger<AuthController> _logger;
        private readonly IAccountService _accountService;

        public AuthController(ILogger<AuthController> logger,
            IAccountService accountService)
        {
            _logger = logger;
            _accountService = accountService;
        }

        [HttpPost("createtoken")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(JsonWebToken), (int)HttpStatusCode.OK)]
        [ProducesDefaultResponseType(typeof(CommandResult))]
        public IActionResult CreateToken([FromBody] CreateTokenRequest request)
        {
            
            _logger.LogInformation(
                "----- Request processing: {CommandName} - {IdProperty}: {CommandId} ({@Command})",
                request.GetGenericTypeName(),
                nameof(request.CustomerId),
                request.CustomerId,
                request);

            var requestResult = _accountService.CrateToken(request.CustomerId);

            return Ok(requestResult);
        }

        [HttpPost("refreshtoken")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(JsonWebToken), (int)HttpStatusCode.OK)]
        [ProducesDefaultResponseType(typeof(CommandResult))]
        public IActionResult RefreshToken([FromBody] RefreshTokenRequest request)
        {
         

            _logger.LogInformation(
                "----- Request processing: {CommandName} - {IdProperty}: {CommandId} ({@Command})",
                request.GetGenericTypeName(),
                nameof(request.CustomerId),
                request.CustomerId,
                request);

            var requestResult = _accountService.RefreshToken(request.CustomerId, request.RefreshToken);

            return Ok(requestResult);
        }

        [HttpPost("loginwithemail")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(JsonWebToken), (int)HttpStatusCode.OK)]
        [ProducesDefaultResponseType(typeof(CommandResult))]
        public IActionResult LoginWithEmail([FromBody] LoginWithEmailRequest request)
        {

            _logger.LogInformation(
                "----- Request processing: {CommandName} - {IdProperty}: {CommandId} ({@Command})",
                request.GetGenericTypeName(),
                nameof(request.Email),
                request.Email,
                request);

            var requestResult = _accountService.LoginWithEmail(request.Email, request.Password);

            return Ok(requestResult);
        }

     
    }
}
