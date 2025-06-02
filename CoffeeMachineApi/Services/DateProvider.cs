namespace CoffeeMachineApi.Services
{
    public interface IDateProvider
    {
        DateTime UtcNow { get; }
    }

    public class DateProvider : IDateProvider
    {
        public DateTime UtcNow => DateTime.UtcNow;
    }
}
