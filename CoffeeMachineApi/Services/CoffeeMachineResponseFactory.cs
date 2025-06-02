using CoffeeMachineApi.Models;

namespace CoffeeMachineApi.Services
{
    public class CoffeeMachineResponseFactory : ICoffeeMachineResponseFactory
    {
        private static readonly Dictionary<CoffeeMachineStatus, (int StatusCode, object Response)> _responses =
    new()
    {
                { CoffeeMachineStatus.Ready, (200, new { message = "Your piping hot coffee is ready", prepared = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:sszzz") }) },
                { CoffeeMachineStatus.OutOfCoffeeMachine, (503, null) },
                { CoffeeMachineStatus.Teapot, (418, null) } 
    };

        public (int StatusCode, object Response) GetResponse(int requestCount, DateTime date)
        {
            var status = date.Month == 4 && date.Day == 1 ? CoffeeMachineStatus.Teapot
                        : requestCount % 5 == 0 ? CoffeeMachineStatus.OutOfCoffeeMachine
                        : CoffeeMachineStatus.Ready;

            return _responses[status];
        }
    }
}
