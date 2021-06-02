using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Ordering.Application.Abstractions;
using Ordering.Application.Commands.OrderCreate;
using Ordering.Application.Commands.OrderPaymentSuccess;
using Ordering.Application.Commands.OrderStatus;
using Ordering.Application.Queries;
using Ordering.Application.Responses;
using Ordering.Domain.Enums;
using Ordering.Domain.Helpers;

namespace Order.EShop.Order.Controller
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<OrderController> _logger;
        public OrderController(IMediator mediator, ILogger<OrderController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }
        [HttpGet("GetOrdersByMerchantNameQuery/{merchantName}")]
        [ProducesResponseType(typeof(IEnumerable<CommandResult>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<IEnumerable<CommandResult>>> GetOrdersByUserName(string merchantName)
        {
            var query = new GetOrdersByMerchantNameQuery(merchantName);

            var orders = await _mediator.Send(query);

            if (orders == null)
            {
                return Ok(CommandResult.GetError(ResponseStatus.Error, "Unexpected Error"));
            }

            return Ok(orders);
        }

        [HttpPost]
        [ProducesResponseType(typeof(CommandResult), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<CommandResult>> OrderCreate([FromBody] OrderCreateCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPost("ChangeOrderStatus")]
        [ProducesResponseType(typeof(CommandResult), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<CommandResult>> ChangeOrderStatus([FromBody] OrderStatusCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }
    }
}
