
using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Payment.Application.Commands.CreatePayment;
using Payment.Application.Responses;

namespace EShop.FakePayment.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class FakePaymentsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<FakePaymentsController> _logger;

        public FakePaymentsController(IMediator mediator, ILogger<FakePaymentsController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpPost]
        [ProducesResponseType(typeof(PaymentResponse), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<PaymentResponse>> PaymentCreate([FromBody] PaymentCreateCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPost]
        public IActionResult Finish()
        {
            return Ok();
        }
    }
}
