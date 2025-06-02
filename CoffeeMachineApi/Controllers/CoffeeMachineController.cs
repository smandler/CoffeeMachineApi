using Microsoft.AspNetCore.Mvc;
using CoffeeMachineApi.Services;

namespace CoffeeMachineAPI.Controllers
{
    /// <summary>
    /// API Controller for handling CoffeeMachine brewing requests.
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class CoffeeMachineController : ControllerBase
    {
        private readonly CoffeeMachineService _coffeeMachineService;
        private readonly ILogger<CoffeeMachineController> _logger;

        /// <summary>
        /// Constructor with dependency injection for the CoffeeMachine machine service and logging.
        /// </summary>
        public CoffeeMachineController(CoffeeMachineService CoffeeMachineService, ILogger<CoffeeMachineController> logger)
        {
            _coffeeMachineService = CoffeeMachineService;
            _logger = logger;
        }

        /// <summary>
        /// GET endpoint to brew coffee.
        /// </summary>
        [HttpGet("brew-Coffee")]
        public IActionResult BrewCoffee()
        {
            _logger.LogInformation("Received request at /brew-coffee");

            var (statusCode, response) = _coffeeMachineService.BrewCoffee();

            _logger.LogInformation("Response sent with status {statusCode}", statusCode);

            return response is null ? StatusCode(statusCode) : StatusCode(statusCode, response);
        }
    }
}
