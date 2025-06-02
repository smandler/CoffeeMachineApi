namespace CoffeeMachineApi.Services
{
    /// <summary>
    /// Service responsible for processing CoffeeMachine requests and logging events.
    /// </summary>
    public class CoffeeMachineService
    {
        private readonly ICoffeeMachineResponseFactory _responseFactory;
        private readonly IDateProvider _dateProvider;
        private readonly ILogger<CoffeeMachineService> _logger;
        private int _requestCount = 0;

        /// <summary>
        /// Constructor with dependency injection of the response factory and logger.
        /// </summary>
        public CoffeeMachineService(ICoffeeMachineResponseFactory responseFactory, IDateProvider dateProvider, ILogger<CoffeeMachineService> logger)
        {
            _responseFactory = responseFactory;
            _dateProvider = dateProvider;
            _logger = logger;
        }

        /// <summary>
        /// Handles CoffeeMachine brewing requests and logs relevant information.
        /// </summary>
        public (int statusCode, object response) BrewCoffee()
        {
            _requestCount++;
            DateTime now = _dateProvider.UtcNow;
            
            var (statusCode, response) = _responseFactory.GetResponse(_requestCount, now);

            _logger.LogInformation("BrewCoffeeMachine called. Request count: {count}, Status: {status}, Date: {date}",
                _requestCount, statusCode, now.ToString("o"));

            if (statusCode == 418)
                _logger.LogWarning("April 1st detected! Returning 418 - I'm a teapot!");

            if (statusCode == 503)
                _logger.LogError("Coffee machine is out of coffee or under maintenance!");

            return (statusCode, response);
        }
    }
}
