using EShop.EventBus.Abstractions;
using Microsoft.AspNetCore.Mvc;


namespace EShop.Mailing.API.Controllers
{
    public class HomeController : Controller
    {
        private readonly IEventBus _eventBus;

        public HomeController(IEventBus eventBus)
        {
            _eventBus = eventBus;
        }

        [Route("/")]
        public IActionResult Index()
        {
            //_eventBus.Publish(new EmailConfirmationEvent("polatosmann@gmail.com", 1, "123111"));

            return Content("Online");
        }
    }
}
