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
        [ProducesResponseType(typeof(IEnumerable<OrderResponse>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<IEnumerable<OrderResponse>>> GetOrdersByUserName(string merchantName)
        {
            var query = new GetOrdersByMerchantNameQuery(merchantName);

            var orders = await _mediator.Send(query);
            if (orders.Count() == decimal.Zero)
                return NotFound();

            return Ok(orders);
        }

        [HttpPost]
        [ProducesResponseType(typeof(OrderResponse), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<OrderResponse>> OrderCreate([FromBody] OrderCreateCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPost("ChangeOrderStatus")]
        [ProducesResponseType(typeof(OrderResponse), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<OrderResponse>> ChangeOrderStatus([FromBody] OrderStatusCommand command)
        {

            //TODO: How can i create only one method for all change of status?

            var requestCommand = new IdentifiedCommand<OrderPaymentSuccessCommand, CommandResult>(command, Guid.NewGuid());

            _logger.LogInformation(
                "----- Sending command: {CommandName} - {IdProperty}: {CommandId} ({@Command})",
                requestCommand.GetGenericTypeName(),
                nameof(requestCommand.Command.OrderId),
                requestCommand.Command.OrderStatus,
                requestCommand);


            var result = await _mediator.Send(command);
            return Ok(result);
        }
    }
}
