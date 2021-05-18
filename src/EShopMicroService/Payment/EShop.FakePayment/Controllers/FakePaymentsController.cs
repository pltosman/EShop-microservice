
using Microsoft.AspNetCore.Mvc;

namespace EShop.FakePayment.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class FakePaymentsController : ControllerBase
    {

        [HttpPost]
        public IActionResult ReceivePayment()
        {
            return Ok();
        }
    }
}
