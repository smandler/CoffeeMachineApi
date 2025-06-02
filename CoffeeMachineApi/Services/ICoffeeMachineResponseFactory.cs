namespace CoffeeMachineApi.Services
{
    /// <summary>
    /// Interface for the factory that generates CoffeeMachine responses based on request count and date.
    /// </summary>
    public interface ICoffeeMachineResponseFactory
    {
        (int StatusCode, object Response) GetResponse(int requestCount, DateTime date);
    }
}
